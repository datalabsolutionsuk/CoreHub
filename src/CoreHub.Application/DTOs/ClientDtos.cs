using CoreHub.Domain.Enums;

namespace CoreHub.Application.DTOs;

// Client DTOs - Read Models Only (Commands are in Features folder)
public record ClientDto
{
    public Guid Id { get; init; }
    public string ClientCode { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public Gender Gender { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public ClientStatus Status { get; init; }
    public Guid? AssignedPractitionerId { get; init; }
    public string? AssignedPractitionerName { get; init; }
    public Guid? ProgramId { get; init; }
    public string? ProgramName { get; init; }
    public DataQualityScore DataQualityScore { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ClosedDate { get; init; }
    public List<string> ActiveFlags { get; init; } = new();
}

public record ClientDetailDto : ClientDto
{
    public string? Address { get; init; }
    public string? PostalCode { get; init; }
    public Guid? SubsiteId { get; init; }
    public string? SubsiteName { get; init; }
    public DateTime? ReferralDate { get; init; }
    public DateTime? FirstAppointmentDate { get; init; }
    public DischargeReason? DischargeReason { get; init; }
    public string? DischargeNotes { get; init; }
    public bool ConsentToContact { get; init; }
    public bool ConsentToResearch { get; init; }
}
