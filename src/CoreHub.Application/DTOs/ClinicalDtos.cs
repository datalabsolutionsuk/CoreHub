using CoreHub.Domain.Enums;

namespace CoreHub.Application.DTOs;

public record FlagDto
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public FlagType Type { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime RaisedAt { get; init; }
    public string RaisedBy { get; init; } = string.Empty;
    public bool IsCleared { get; init; }
    public DateTime? ClearedAt { get; init; }
    public string? ClearedBy { get; init; }
}

public record RaiseFlagCommand
{
    public Guid ClientId { get; init; }
    public FlagType Type { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public record ClearFlagCommand
{
    public Guid Id { get; init; }
    public string? Note { get; init; }
}

public record CaseNoteDto
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public Guid? SessionId { get; init; }
    public NoteCategory Category { get; init; }
    public bool IsCritical { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public bool IsLocked { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
}

public record CreateNoteCommand
{
    public Guid ClientId { get; init; }
    public Guid? SessionId { get; init; }
    public NoteCategory Category { get; init; }
    public bool IsCritical { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}
