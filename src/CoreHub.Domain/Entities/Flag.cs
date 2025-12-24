using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class ClientFlag : TenantEntity
{
    public Guid ClientId { get; set; }
    public FlagType Type { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime RaisedAt { get; set; }
    public string RaisedBy { get; set; } = string.Empty;
    public bool IsCleared { get; set; }
    public DateTime? ClearedAt { get; set; }
    public string? ClearedBy { get; set; }
    public string? ClearanceNote { get; set; }
    public string? Metadata { get; set; } // JSON for additional context
    
    public Client Client { get; set; } = null!;
}

public class FlagRule : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public FlagType FlagType { get; set; }
    public bool IsActive { get; set; } = true;
    public string Condition { get; set; } = string.Empty; // JSON rule definition
    public string? Description { get; set; }
    public int Priority { get; set; }
}
