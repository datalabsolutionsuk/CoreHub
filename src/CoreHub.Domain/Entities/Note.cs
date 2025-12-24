using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class CaseNote : TenantEntity
{
    public Guid ClientId { get; set; }
    public Guid? SessionId { get; set; }
    public NoteCategory Category { get; set; }
    public bool IsCritical { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsLocked { get; set; }
    public DateTime? LockedAt { get; set; }
    public string? LockedBy { get; set; }
    
    public Client Client { get; set; } = null!;
    public Session? Session { get; set; }
}

public class Document : TenantEntity
{
    public Guid ClientId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string BlobReference { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    
    public Client Client { get; set; } = null!;
}
