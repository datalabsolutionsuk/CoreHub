using CoreHub.Application.Features.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreHub.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("kpi")]
    public async Task<IActionResult> GetKpiReport([FromBody] GetKpiReportQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("scatter")]
    public async Task<IActionResult> GetScatterPlot([FromBody] GetScatterPlotDataQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("progress")]
    public async Task<IActionResult> GetProgressChart([FromQuery] Guid clientId, [FromQuery] Guid measureId)
    {
        var result = await _mediator.Send(new GetProgressChartQuery 
        { 
            ClientId = clientId, 
            MeasureId = measureId 
        });
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("export")]
    public async Task<IActionResult> ExportReport([FromBody] ExportReportCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess) return BadRequest(result.Error);
        
        var contentType = command.Format == "PDF" 
            ? "application/pdf" 
            : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        
        var fileName = $"report_{DateTime.UtcNow:yyyyMMdd}.{command.Format.ToLower()}";
        
        return File(result.Data!, contentType, fileName);
    }
}
