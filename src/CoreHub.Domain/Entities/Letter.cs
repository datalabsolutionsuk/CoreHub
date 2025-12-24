using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class LetterTemplate : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string BodyHtml { get; set; } = string.Empty;
    public string? Category { get; set; }
    public bool IsActive { get; set; } = true;
    public string? MergeFields { get; set; } // JSON list of available fields
    
    public ICollection<Letter> Letters { get; set; } = new List<Letter>();
}

public class Letter : TenantEntity
{
    public Guid ClientId { get; set; }
    public Guid TemplateId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string BodyHtml { get; set; } = string.Empty;
    public string? RenderedPdfBlobRef { get; set; }
    public LetterStatus Status { get; set; } = LetterStatus.Draft;
    public DateTime? SentAt { get; set; }
    public string? SentBy { get; set; }
    public string? RecipientEmail { get; set; }
    
    public Client Client { get; set; } = null!;
    public LetterTemplate Template { get; set; } = null!;
}
