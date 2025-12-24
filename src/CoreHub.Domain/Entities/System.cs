using CoreHub.Domain.Common;

namespace CoreHub.Domain.Entities;

public class SystemSetting : TenantEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
}

public class DataQualityRequirement : TenantEntity
{
    public Guid? ProgramId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? Stage { get; set; } // "Intake", "Treatment", "Discharge"
    public int ScoreWeight { get; set; } = 1;
    
    public Program? Program { get; set; }
}

public class ReportPreset : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty; // "KPI", "Activity", "Scatter"
    public string Filters { get; set; } = "{}"; // JSON filters
    public Guid UserId { get; set; }
    public bool IsShared { get; set; }
}

public class AuditLog : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public AuditAction Action { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}

public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    View = 4,
    Export = 5
}
