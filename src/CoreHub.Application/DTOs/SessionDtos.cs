using CoreHub.Domain.Enums;

namespace CoreHub.Application.DTOs;

public record SessionDto
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public Guid PractitionerId { get; init; }
    public string PractitionerName { get; init; } = string.Empty;
    public DateTime SessionDate { get; init; }
    public SessionType Type { get; init; }
    public int SessionNumber { get; init; }
    public int? DurationMinutes { get; init; }
    public bool DNA { get; init; }
    public int FormCount { get; init; }
}

public record CreateSessionCommand
{
    public Guid ClientId { get; init; }
    public Guid PractitionerId { get; init; }
    public DateTime SessionDate { get; init; }
    public SessionType Type { get; init; }
    public int? DurationMinutes { get; init; }
    public string? Summary { get; init; }
}

public record AppointmentDto
{
    public Guid Id { get; init; }
    public Guid? ClientId { get; init; }
    public string? ClientName { get; init; }
    public Guid PractitionerId { get; init; }
    public string PractitionerName { get; init; } = string.Empty;
    public Guid? RoomId { get; init; }
    public string? RoomName { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public AppointmentStatus Status { get; init; }
    public string? Title { get; init; }
    public bool ReminderSent { get; init; }
}

public record CreateAppointmentCommand
{
    public Guid? ClientId { get; init; }
    public Guid PractitionerId { get; init; }
    public Guid? RoomId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string? Title { get; init; }
    public string? Notes { get; init; }
}

public record UpdateAppointmentCommand
{
    public Guid Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public Guid? RoomId { get; init; }
    public string? Notes { get; init; }
}

public record CancelAppointmentCommand
{
    public Guid Id { get; init; }
    public string? Reason { get; init; }
}
