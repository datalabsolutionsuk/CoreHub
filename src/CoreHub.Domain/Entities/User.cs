using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class User : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public UserRole Role { get; set; }
    public string IdentityUserId { get; set; } = string.Empty; // Link to ASP.NET Identity
    public bool IsActive { get; set; } = true;
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    
    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
    public ICollection<Client> AssignedClients { get; set; } = new List<Client>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

public class Team : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ManagerId { get; set; }
    
    public User? Manager { get; set; }
    public ICollection<UserTeam> UserTeams { get; set; } = new List<UserTeam>();
}

public class UserTeam : TenantEntity
{
    public Guid UserId { get; set; }
    public Guid TeamId { get; set; }
    
    public User User { get; set; } = null!;
    public Team Team { get; set; } = null!;
}
