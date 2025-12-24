using CoreHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreHub.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(t => t.Slug).IsUnique();
        
        builder.Property(t => t.Settings)
            .HasColumnType("nvarchar(max)");
        
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
        
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.ClientCode)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.Demographics)
            .HasColumnType("nvarchar(max)");
        
        builder.HasOne(c => c.AssignedPractitioner)
            .WithMany(u => u.AssignedClients)
            .HasForeignKey(c => c.AssignedPractitionerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.Program)
            .WithMany(p => p.Clients)
            .HasForeignKey(c => c.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(c => new { c.TenantId, c.ClientCode }).IsUnique();
        builder.HasIndex(c => c.AssignedPractitionerId);
        builder.HasIndex(c => c.Status);
        
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}

public class MeasureConfiguration : IEntityTypeConfiguration<Measure>
{
    public void Configure(EntityTypeBuilder<Measure> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(m => m.ScoringRules)
            .HasColumnType("nvarchar(max)");
        
        builder.HasMany(m => m.Items)
            .WithOne(i => i.Measure)
            .HasForeignKey(i => i.MeasureId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}

public class MeasureItemConfiguration : IEntityTypeConfiguration<MeasureItem>
{
    public void Configure(EntityTypeBuilder<MeasureItem> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.ItemCode)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(i => i.QuestionText)
            .IsRequired()
            .HasMaxLength(1000);
        
        builder.Property(i => i.Options)
            .HasColumnType("nvarchar(max)");
        
        builder.HasIndex(i => new { i.MeasureId, i.ItemNumber });
    }
}

public class AdministeredFormConfiguration : IEntityTypeConfiguration<AdministeredForm>
{
    public void Configure(EntityTypeBuilder<AdministeredForm> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.AccessToken)
            .HasMaxLength(500);
        
        builder.Property(f => f.SubscaleScores)
            .HasColumnType("nvarchar(max)");
        
        builder.Property(f => f.InterpretationFlags)
            .HasColumnType("nvarchar(max)");
        
        builder.HasOne(f => f.Client)
            .WithMany(c => c.Forms)
            .HasForeignKey(f => f.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(f => f.Measure)
            .WithMany(m => m.AdministeredForms)
            .HasForeignKey(f => f.MeasureId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(f => f.AccessToken);
        builder.HasIndex(f => new { f.ClientId, f.SubmittedAt });
        
        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.HasOne(s => s.Client)
            .WithMany(c => c.Sessions)
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(s => s.Practitioner)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.PractitionerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(s => new { s.ClientId, s.SessionDate });
        
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}

public class ClientFlagConfiguration : IEntityTypeConfiguration<ClientFlag>
{
    public void Configure(EntityTypeBuilder<ClientFlag> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Reason)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(f => f.Metadata)
            .HasColumnType("nvarchar(max)");
        
        builder.HasOne(f => f.Client)
            .WithMany(c => c.Flags)
            .HasForeignKey(f => f.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(f => new { f.ClientId, f.IsCleared });
        builder.HasIndex(f => f.Type);
        
        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.HasOne(a => a.Client)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a => a.Practitioner)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.PractitionerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a => a.Room)
            .WithMany(r => r.Appointments)
            .HasForeignKey(a => a.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(a => new { a.PractitionerId, a.StartTime });
        builder.HasIndex(a => a.StartTime);
        
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}

public class CaseNoteConfiguration : IEntityTypeConfiguration<CaseNote>
{
    public void Configure(EntityTypeBuilder<CaseNote> builder)
    {
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Subject)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(n => n.Body)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
        
        builder.HasOne(n => n.Client)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(n => new { n.ClientId, n.CreatedAt });
        
        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}
