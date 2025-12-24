namespace CoreHub.Domain.ValueObjects;

public record Address(
    string? Street,
    string? City,
    string? State,
    string? PostalCode,
    string? Country
);

public record ContactInfo(
    string? Email,
    string? Phone,
    string? AlternatePhone,
    bool PreferEmail,
    bool PreferSMS
);

public record ScoreResult(
    decimal TotalScore,
    Dictionary<string, decimal> SubscaleScores,
    List<string> Flags,
    string? Interpretation
);

public record DateRange(
    DateTime StartDate,
    DateTime EndDate
)
{
    public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;
    public int DurationDays => (EndDate - StartDate).Days;
}

public record MeasureScore(
    Guid MeasureId,
    string MeasureName,
    decimal Value,
    DateTime AssessedAt,
    bool IsBaseline
);
