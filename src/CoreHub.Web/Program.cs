using CoreHub.Infrastructure.Persistence;
using CoreHub.Infrastructure.Identity;
using CoreHub.Application.Interfaces;
using CoreHub.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Hangfire;
using Hangfire.SqlServer;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/corehub-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

// Register IApplicationDbContext interface
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

// Register ICurrentUserService
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    // Super Admin - Full access to everything
    options.AddPolicy("SuperAdminOnly", policy => 
        policy.RequireRole("SuperAdmin"));
    
    // Admin and above
    options.AddPolicy("AdminAccess", policy => 
        policy.RequireRole("SuperAdmin", "Admin"));
    
    options.AddPolicy("CanViewOwnCases", policy => 
        policy.RequireRole("Practitioner", "Manager", "Admin", "SuperAdmin"));
    options.AddPolicy("CanViewTeamCases", policy => 
        policy.RequireRole("Manager", "Admin", "SuperAdmin"));
    options.AddPolicy("CanViewAllCases", policy => 
        policy.RequireRole("Admin", "SuperAdmin"));
    options.AddPolicy("CanConfigureSystem", policy => 
        policy.RequireRole("Admin", "SuperAdmin"));
    
    // Super Admin exclusive policies
    options.AddPolicy("CanManageUsers", policy => 
        policy.RequireRole("SuperAdmin"));
    options.AddPolicy("CanManageRoles", policy => 
        policy.RequireRole("SuperAdmin"));
    options.AddPolicy("CanDeleteAnything", policy => 
        policy.RequireRole("SuperAdmin"));
    options.AddPolicy("CanViewAuditLogs", policy => 
        policy.RequireRole("SuperAdmin"));
});

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CoreHub.Application.Features.Clients.Commands.CreateClientCommand).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CoreHub.Application.Validators.CreateClientCommandValidator).Assembly);

// Application Services
builder.Services.AddScoped<IScoringService, ScoringService>();
builder.Services.AddScoped<IFlagRulesEngine, FlagRulesEngine>();
builder.Services.AddScoped<IDataQualityCalculator, DataQualityCalculator>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<ILetterService, LetterService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

// Setup Service
builder.Services.AddScoped<CoreHub.Web.Services.SetupService>();

// Hangfire (skip in test environments)
if (builder.Configuration.GetValue<bool>("Hangfire:Enabled", true))
{
    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddHangfireServer();
    }

// SignalR
builder.Services.AddSignalR();

// Blazorise
builder.Services
    .AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// API Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Checks
var healthChecks = builder.Services.AddHealthChecks();
if (builder.Configuration.GetValue<bool>("Hangfire:Enabled", true))
{
    healthChecks.AddHangfire(options => options.MinimumAvailableServers = 1);
}

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Comment out HTTPS redirection for development
// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Map endpoints
app.MapControllers();
app.MapRazorComponents<CoreHub.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.MapHealthChecks("/health");

// Initialize database and seed Super Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await ApplicationDbContextSeed.InitializeAsync(context, services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Hangfire Dashboard (skip in test environments)
if (app.Configuration.GetValue<bool>("Hangfire:Enabled", true))
{
    app.MapHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter() }
    });
}

// SignalR Hubs
app.MapHub<CoreHub.Web.Hubs.NotificationHub>("/hubs/notifications");

app.Run();

// Hangfire Authorization Filter
public class HangfireAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
{
    public bool Authorize(Hangfire.Dashboard.DashboardContext context)
    {
        // In production, check if user is Admin
        return true;
    }
}

// Make Program class accessible for integration tests
public partial class Program { }
