using CoreHub.Application.Common;
using CoreHub.Application.DTOs;
using MediatR;

namespace CoreHub.Application.Features.Reports.Queries;

public record GetKpiReportQuery : IRequest<Result<KpiReportDto>>
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public Guid? ProgramId { get; init; }
    public Guid? PractitionerId { get; init; }
}

public record GetScatterPlotDataQuery : IRequest<Result<List<ScatterPlotDataDto>>>
{
    public Guid MeasureId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public Guid? ProgramId { get; init; }
    public int? MinSessions { get; init; } = 2;
}

public record GetProgressChartQuery : IRequest<Result<ProgressChartDto>>
{
    public Guid ClientId { get; init; }
    public Guid MeasureId { get; init; }
}

public record ExportReportCommand : IRequest<Result<byte[]>>
{
    public string ReportType { get; init; } = string.Empty;
    public string Format { get; init; } = "Excel";
    public Dictionary<string, object> Parameters { get; init; } = new();
}
