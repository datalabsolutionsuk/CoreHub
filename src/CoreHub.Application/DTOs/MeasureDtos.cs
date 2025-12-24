using CoreHub.Domain.Enums;

namespace CoreHub.Application.DTOs;

public record MeasureDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Version { get; init; }
    public string? Description { get; init; }
    public bool IsStandard { get; init; }
    public int ItemCount { get; init; }
}

public record MeasureDetailDto : MeasureDto
{
    public List<MeasureItemDto> Items { get; init; } = new();
    public List<MeasureSubscaleDto> Subscales { get; init; } = new();
}

public record MeasureItemDto
{
    public Guid Id { get; init; }
    public int ItemNumber { get; init; }
    public string ItemCode { get; init; } = string.Empty;
    public string QuestionText { get; init; } = string.Empty;
    public MeasureScaleType ScaleType { get; init; }
    public bool IsReverseScored { get; init; }
    public bool IsRiskItem { get; init; }
}

public record MeasureSubscaleDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int? MinScore { get; init; }
    public int? MaxScore { get; init; }
}

public record AdministeredFormDto
{
    public Guid Id { get; init; }
    public Guid ClientId { get; init; }
    public Guid MeasureId { get; init; }
    public string MeasureName { get; init; } = string.Empty;
    public Guid? SessionId { get; init; }
    public FormMode Mode { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime? SubmittedAt { get; init; }
    public decimal? TotalScore { get; init; }
    public Dictionary<string, decimal>? SubscaleScores { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record FormLinkDto
{
    public string Url { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}
