using CoreHub.Application.Features.Measures.Commands;
using CoreHub.Application.Features.Measures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreHub.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasuresController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeasuresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetMeasures([FromQuery] GetMeasuresQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetMeasure(Guid id)
    {
        var result = await _mediator.Send(new GetMeasureDetailQuery { MeasureId = id });
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost("forms")]
    [Authorize]
    public async Task<IActionResult> CreateForm([FromBody] CreateFormCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("forms/{id}/submit")]
    public async Task<IActionResult> SubmitForm(Guid id, [FromBody] SubmitFormCommand command)
    {
        if (id != command.FormId) return BadRequest("ID mismatch");
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("forms/token/{token}")]
    public async Task<IActionResult> GetFormByToken(string token)
    {
        var result = await _mediator.Send(new GetFormByTokenQuery { Token = token });
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost("forms/{id}/link")]
    [Authorize]
    public async Task<IActionResult> GenerateFormLink(Guid id, [FromQuery] int expiryHours = 72)
    {
        var result = await _mediator.Send(new GenerateFormLinkCommand 
        { 
            FormId = id, 
            ExpiryHours = expiryHours 
        });
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }
}
