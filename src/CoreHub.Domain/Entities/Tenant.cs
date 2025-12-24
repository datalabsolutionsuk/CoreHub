using CoreHub.Domain.Common;

namespace CoreHub.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public DateTime? SubscriptionExpiresAt { get; set; }
    public string Settings { get; set; } = "{}"; // JSON settings
    
    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Program> Programs { get; set; } = new List<Program>();
}
