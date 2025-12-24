using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class Session : TenantEntity
{
    public Guid ClientId { get; set; }
    public Guid PractitionerId { get; set; }
    public DateTime SessionDate { get; set; }
    public SessionType Type { get; set; }
    public int SessionNumber { get; set; }
    public int? DurationMinutes { get; set; }
    public bool DNA { get; set; }
    public string? DNAReason { get; set; }
    public string? Summary { get; set; }
    
    public Client Client { get; set; } = null!;
    public User Practitioner { get; set; } = null!;
    public ICollection<AdministeredForm> Forms { get; set; } = new List<AdministeredForm>();
    public ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();
}
