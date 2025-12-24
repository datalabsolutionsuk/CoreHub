namespace CoreHub.Application.DTOs;

public record KpiReportDto
{
    public DateRange Period { get; init; } = null!;
    public int TotalClients { get; init; }
    public int NewReferrals { get; init; }
    public int ActiveClients { get; init; }
    public int DischargedClients { get; init; }
    public decimal ImprovementRate { get; init; }
    public int TotalSessions { get; init; }
    public decimal AverageSessionsPerClient { get; init; }
    public int DNACount { get; init; }
    public decimal DNARate { get; init; }
    public int RiskFlagsCount { get; init; }
    public int OffTrackFlagsCount { get; init; }
    public Dictionary<string, int> DischargeReasons { get; init; } = new();
    public decimal AverageWaitDays { get; init; }
}

public record ScatterPlotDataDto
{
    public Guid ClientId { get; init; }
    public string ClientCode { get; init; } = string.Empty;
    public decimal FirstScore { get; init; }
    public decimal LastScore { get; init; }
    public DateTime FirstAssessment { get; init; }
    public DateTime LastAssessment { get; init; }
    public int SessionCount { get; init; }
}

public record ProgressChartDto
{
    public Guid ClientId { get; init; }
    public Guid MeasureId { get; init; }
    public string MeasureName { get; init; } = string.Empty;
    public List<DataPoint> DataPoints { get; init; } = new();
}

public record DataPoint
{
    public DateTime Date { get; init; }
    public decimal Score { get; init; }
    public int? SessionNumber { get; init; }
}

public record DateRange
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}

public record GenerateReportCommand
{
    public string ReportType { get; init; } = string.Empty; // KPI, Activity, Scatter
    public DateRange? DateRange { get; init; }
    public Guid? ProgramId { get; init; }
    public Guid? PractitionerId { get; init; }
    public Guid? MeasureId { get; init; }
    public string ExportFormat { get; init; } = "Excel"; // Excel, PDF
}

public record ExportDataCommand
{
    public string EntityType { get; init; } = string.Empty; // Clients, Sessions, Forms
    public Dictionary<string, object> Filters { get; init; } = new();
    public string Format { get; init; } = "Excel";
}
