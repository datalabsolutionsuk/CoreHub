using Bogus;
using CoreHub.Domain.Entities;
using CoreHub.Domain.Enums;
using CoreHub.Infrastructure.Persistence;
using CoreHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<ApplicationDbContext>();
var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

Console.WriteLine("CoreHub Seed Tool");
Console.WriteLine("=================");
Console.WriteLine();

// Ensure database is created
await context.Database.MigrateAsync();
Console.WriteLine("✓ Database migrated");

// Seed Tenant
var tenant = new Tenant
{
    Id = Guid.NewGuid(),
    Name = "DemoCare",
    Slug = "democare",
    IsActive = true,
    ContactEmail = "admin@democare.com",
    CreatedAt = DateTime.UtcNow,
    CreatedBy = "seed"
};

if (!await context.Tenants.AnyAsync())
{
    context.Tenants.Add(tenant);
    await context.SaveChangesAsync();
    Console.WriteLine($"✓ Created tenant: {tenant.Name}");
}
else
{
    tenant = await context.Tenants.FirstAsync();
    Console.WriteLine($"✓ Using existing tenant: {tenant.Name}");
}

// Seed Identity Users and Application Users
var identityUsers = new List<(ApplicationUser Identity, User App, string Password)>
{
    (new ApplicationUser 
    { 
        UserName = "admin@demo.com", 
        Email = "admin@demo.com", 
        FirstName = "Admin", 
        LastName = "User",
        TenantId = tenant.Id,
        EmailConfirmed = true
    }, new User 
    { 
        TenantId = tenant.Id, 
        Email = "admin@demo.com", 
        FirstName = "Admin", 
        LastName = "User", 
        Role = UserRole.Admin,
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "seed"
    }, "Admin123!"),
    
    (new ApplicationUser 
    { 
        UserName = "manager@demo.com", 
        Email = "manager@demo.com", 
        FirstName = "Manager", 
        LastName = "User",
        TenantId = tenant.Id,
        EmailConfirmed = true
    }, new User 
    { 
        TenantId = tenant.Id, 
        Email = "manager@demo.com", 
        FirstName = "Manager", 
        LastName = "User", 
        Role = UserRole.Manager,
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "seed"
    }, "Manager123!"),
    
    (new ApplicationUser 
    { 
        UserName = "therapist1@demo.com", 
        Email = "therapist1@demo.com", 
        FirstName = "Sarah", 
        LastName = "Therapist",
        TenantId = tenant.Id,
        EmailConfirmed = true
    }, new User 
    { 
        TenantId = tenant.Id, 
        Email = "therapist1@demo.com", 
        FirstName = "Sarah", 
        LastName = "Therapist", 
        Role = UserRole.Practitioner,
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "seed"
    }, "Therapist123!")
};

var appUsers = new List<User>();

foreach (var (identityUser, userEntity, password) in identityUsers)
{
    var existingIdentity = await userManager.FindByEmailAsync(identityUser.Email!);
    User appUser;
    if (existingIdentity == null)
    {
        var result = await userManager.CreateAsync(identityUser, password);
        if (result.Succeeded)
        {
            userEntity.IdentityUserId = identityUser.Id;
            context.Users.Add(userEntity);
            Console.WriteLine($"✓ Created user: {identityUser.Email}");
            appUser = userEntity;
        }
        else
        {
            continue;
        }
    }
    else
    {
        appUser = await context.Users.FirstAsync(u => u.Email == identityUser.Email);
        Console.WriteLine($"✓ Using existing user: {identityUser.Email}");
    }
    appUsers.Add(appUser);
}

await context.SaveChangesAsync();

// Seed Programs
var programs = new List<CoreHub.Domain.Entities.Program>
{
    new() { TenantId = tenant.Id, Name = "Primary Care", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "seed" },
    new() { TenantId = tenant.Id, Name = "Students", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "seed" },
    new() { TenantId = tenant.Id, Name = "Workplace", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "seed" }
};

if (!await context.Programs.AnyAsync())
{
    context.Programs.AddRange(programs);
    await context.SaveChangesAsync();
    Console.WriteLine($"✓ Created {programs.Count} programs");
}

// Seed CORE-10 Measure
if (!await context.Measures.AnyAsync())
{
    var core10 = new Measure
    {
        TenantId = tenant.Id,
        Name = "CORE-10",
        Version = "1.0",
        Description = "Clinical Outcomes in Routine Evaluation - 10 item",
        IsStandard = true,
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "seed"
    };

    var items = new[]
    {
        "I have felt tense, anxious or nervous",
        "I have felt I have someone to turn to for support when needed",
        "I have felt able to cope when things go wrong",
        "Talking to people has felt too much for me",
        "I have felt panic or terror",
        "I made plans to end my life",
        "I have had difficulty getting to sleep or staying asleep",
        "I have felt despairing or hopeless",
        "I have felt unhappy",
        "Unwanted images or memories have been distressing me"
    };

    for (int i = 0; i < items.Length; i++)
    {
        core10.Items.Add(new MeasureItem
        {
            TenantId = tenant.Id,
            ItemNumber = i + 1,
            ItemCode = $"CORE10_{i + 1:D2}",
            QuestionText = items[i],
            ScaleType = MeasureScaleType.Likert0To4,
            IsReverseScored = i == 1 || i == 2, // Items 2 & 3 are reverse scored
            IsRiskItem = i == 5, // Item 6 is risk
            RiskThreshold = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "seed"
        });
    }

    context.Measures.Add(core10);
    await context.SaveChangesAsync();
    Console.WriteLine("✓ Created CORE-10 measure");
}

// Seed Clients using Bogus
var practitioner = appUsers.First(u => u.Role == UserRole.Practitioner);
var program = programs.First();

var clientFaker = new Faker<Client>()
    .RuleFor(c => c.TenantId, tenant.Id)
    .RuleFor(c => c.ClientCode, f => $"C{f.Random.Number(1000, 9999)}")
    .RuleFor(c => c.FirstName, f => f.Name.FirstName())
    .RuleFor(c => c.LastName, f => f.Name.LastName())
    .RuleFor(c => c.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
    .RuleFor(c => c.Gender, f => f.PickRandom<Gender>())
    .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
    .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
    .RuleFor(c => c.Address, f => f.Address.StreetAddress())
    .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
    .RuleFor(c => c.Status, f => f.PickRandom<ClientStatus>())
    .RuleFor(c => c.AssignedPractitionerId, practitioner.Id)
    .RuleFor(c => c.ProgramId, program.Id)
    .RuleFor(c => c.ReferralDate, f => f.Date.Past(1))
    .RuleFor(c => c.DataQualityScore, f => f.PickRandom<DataQualityScore>())
    .RuleFor(c => c.ConsentToContact, true)
    .RuleFor(c => c.ConsentToResearch, f => f.Random.Bool())
    .RuleFor(c => c.CreatedAt, DateTime.UtcNow)
    .RuleFor(c => c.CreatedBy, "seed");

if (!await context.Clients.AnyAsync())
{
    var clients = clientFaker.Generate(50);
    context.Clients.AddRange(clients);
    await context.SaveChangesAsync();
    Console.WriteLine($"✓ Created {clients.Count} demo clients");
}

Console.WriteLine();
Console.WriteLine("Seeding complete!");
Console.WriteLine();
Console.WriteLine("Demo Credentials:");
Console.WriteLine("-----------------");
Console.WriteLine("Admin:      admin@demo.com / Admin123!");
Console.WriteLine("Manager:    manager@demo.com / Manager123!");
Console.WriteLine("Therapist:  therapist1@demo.com / Therapist123!");
