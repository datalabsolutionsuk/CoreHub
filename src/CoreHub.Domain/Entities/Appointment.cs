using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class Appointment : TenantEntity
{
    public Guid? ClientId { get; set; }
    public Guid PractitionerId { get; set; }
    public Guid? RoomId { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;
    
    public string? Title { get; set; }
    public string? Notes { get; set; }
    public bool ReminderSent { get; set; }
    public DateTime? ReminderSentAt { get; set; }
    public string? CancellationReason { get; set; }
    
    public Client? Client { get; set; }
    public User Practitioner { get; set; } = null!;
    public Room? Room { get; set; }
}

public class Room : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int Capacity { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public string? Equipment { get; set; } // JSON list
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
