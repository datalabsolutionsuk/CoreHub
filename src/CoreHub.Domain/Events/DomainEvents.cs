namespace CoreHub.Domain.Events;

public abstract record DomainEvent(DateTime OccurredAt);

public record ClientCreatedEvent(
    Guid ClientId,
    Guid TenantId,
    string ClientCode,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record ClientClosedEvent(
    Guid ClientId,
    Guid TenantId,
    DateTime ClosedAt,
    string? Reason,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record FlagRaisedEvent(
    Guid FlagId,
    Guid ClientId,
    Guid TenantId,
    string FlagType,
    string Reason,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record FlagClearedEvent(
    Guid FlagId,
    Guid ClientId,
    Guid TenantId,
    string ClearedBy,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record FormSubmittedEvent(
    Guid FormId,
    Guid ClientId,
    Guid MeasureId,
    Guid TenantId,
    decimal? Score,
    bool RiskDetected,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record AppointmentBookedEvent(
    Guid AppointmentId,
    Guid? ClientId,
    Guid PractitionerId,
    DateTime StartTime,
    Guid TenantId,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record AppointmentCancelledEvent(
    Guid AppointmentId,
    string? Reason,
    Guid TenantId,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);
