using CoreHub.Application.Interfaces;
using CoreHub.Domain.Entities;
using CoreHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreHub.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Domain entities
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<UserTeam> UserTeams => Set<UserTeam>();
    
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Subsite> Subsites => Set<Subsite>();
    
    public DbSet<Measure> Measures => Set<Measure>();
    public DbSet<MeasureItem> MeasureItems => Set<MeasureItem>();
    public DbSet<MeasureSubscale> MeasureSubscales => Set<MeasureSubscale>();
    public DbSet<AdministeredForm> AdministeredForms => Set<AdministeredForm>();
    public DbSet<AdministeredAnswer> AdministeredAnswers => Set<AdministeredAnswer>();
    
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<ClientFlag> ClientFlags => Set<ClientFlag>();
    public DbSet<FlagRule> FlagRules => Set<FlagRule>();
    
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Room> Rooms => Set<Room>();
    
    public DbSet<CaseNote> CaseNotes => Set<CaseNote>();
    public DbSet<Document> Documents => Set<Document>();
    
    public DbSet<LetterTemplate> LetterTemplates => Set<LetterTemplate>();
    public DbSet<Letter> Letters => Set<Letter>();
    
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<DataQualityRequirement> DataQualityRequirements => Set<DataQualityRequirement>();
    public DbSet<ReportPreset> ReportPresets => Set<ReportPreset>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Global query filters for soft delete and multi-tenancy
        // These will be applied in configurations
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Handle audit fields
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Common.BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

        foreach (var entry in entries)
        {
            var entity = (Domain.Common.BaseEntity)entry.Entity;
            
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = "system"; // TODO: Get from ICurrentUserService
            }
            
            if (entry.State == EntityState.Modified)
            {
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedBy = "system"; // TODO: Get from ICurrentUserService
            }
            
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                entity.DeletedBy = "system"; // TODO: Get from ICurrentUserService
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
