using CoreHub.Domain.Common;
using CoreHub.Domain.Enums;

namespace CoreHub.Domain.Entities;

public class Measure : TenantEntity
{
    public string Name { get; set; } = string.Empty; // e.g., "CORE-10", "PHQ-9"
    public string? Version { get; set; }
    public string? Description { get; set; }
    public bool IsStandard { get; set; } = true; // vs custom measure
    public bool IsActive { get; set; } = true;
    public string? Copyright { get; set; }
    public string? ScoringRules { get; set; } // JSON describing scoring logic
    
    public ICollection<MeasureItem> Items { get; set; } = new List<MeasureItem>();
    public ICollection<MeasureSubscale> Subscales { get; set; } = new List<MeasureSubscale>();
    public ICollection<AdministeredForm> AdministeredForms { get; set; } = new List<AdministeredForm>();
}

public class MeasureItem : TenantEntity
{
    public Guid MeasureId { get; set; }
    public int ItemNumber { get; set; }
    public string ItemCode { get; set; } = string.Empty; // e.g., "CORE_01"
    public string QuestionText { get; set; } = string.Empty;
    public MeasureScaleType ScaleType { get; set; }
    public bool IsReverseScored { get; set; }
    public bool IsRiskItem { get; set; }
    public int? RiskThreshold { get; set; }
    public string? Options { get; set; } // JSON for response options
    
    public Measure Measure { get; set; } = null!;
    public ICollection<MeasureItemSubscale> Subscales { get; set; } = new List<MeasureItemSubscale>();
}

public class MeasureSubscale : TenantEntity
{
    public Guid MeasureId { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Well-being", "Problems"
    public string? Description { get; set; }
    public int? MinScore { get; set; }
    public int? MaxScore { get; set; }
    
    public Measure Measure { get; set; } = null!;
    public ICollection<MeasureItemSubscale> Items { get; set; } = new List<MeasureItemSubscale>();
}

public class MeasureItemSubscale : BaseEntity
{
    public Guid MeasureItemId { get; set; }
    public Guid SubscaleId { get; set; }
    
    public MeasureItem MeasureItem { get; set; } = null!;
    public MeasureSubscale Subscale { get; set; } = null!;
}

public class AdministeredForm : TenantEntity
{
    public Guid ClientId { get; set; }
    public Guid MeasureId { get; set; }
    public Guid? SessionId { get; set; }
    
    public FormMode Mode { get; set; }
    public string? AccessToken { get; set; } // For remote completion
    public DateTime? TokenExpiresAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? SubmittedAt { get; set; }
    
    public decimal? TotalScore { get; set; }
    public string? SubscaleScores { get; set; } // JSON: {"wellbeing": 12, "problems": 8}
    public string? InterpretationFlags { get; set; } // JSON: {"riskDetected": true}
    
    public Client Client { get; set; } = null!;
    public Measure Measure { get; set; } = null!;
    public Session? Session { get; set; }
    public ICollection<AdministeredAnswer> Answers { get; set; } = new List<AdministeredAnswer>();
}

public class AdministeredAnswer : BaseEntity
{
    public Guid AdministeredFormId { get; set; }
    public Guid MeasureItemId { get; set; }
    public int ResponseValue { get; set; }
    public string? ResponseText { get; set; }
    
    public AdministeredForm AdministeredForm { get; set; } = null!;
    public MeasureItem MeasureItem { get; set; } = null!;
}
