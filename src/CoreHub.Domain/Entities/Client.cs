using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class Client : TenantEntity
{
    public string ClientCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string Demographics { get; set; } = "{}"; // JSON for flexible demographics
    
    public Guid? AssignedPractitionerId { get; set; }
    public Guid? ProgramId { get; set; }
    public Guid? SubsiteId { get; set; }
    
    public ClientStatus Status { get; set; } = ClientStatus.Open;
    public DateTime? ReferralDate { get; set; }
    public DateTime? FirstAppointmentDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public DischargeReason? DischargeReason { get; set; }
    public string? DischargeNotes { get; set; }
    
    public DataQualityScore DataQualityScore { get; set; } = DataQualityScore.None;
    public DateTime? DataQualityLastCalculated { get; set; }
    
    public bool ConsentToContact { get; set; }
    public bool ConsentToResearch { get; set; }
    
    // Navigation properties
    public User? AssignedPractitioner { get; set; }
    public Program? Program { get; set; }
    public Subsite? Subsite { get; set; }
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<ClientFlag> Flags { get; set; } = new List<ClientFlag>();
    public ICollection<AdministeredForm> Forms { get; set; } = new List<AdministeredForm>();
    public ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Letter> Letters { get; set; } = new List<Letter>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}

public class Program : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Settings { get; set; } // Program-specific settings like DQ rules
    
    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Subsite> Subsites { get; set; } = new List<Subsite>();
}

public class Subsite : TenantEntity
{
    public Guid ProgramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    
    public Program Program { get; set; } = null!;
    public ICollection<Client> Clients { get; set; } = new List<Client>();
}
