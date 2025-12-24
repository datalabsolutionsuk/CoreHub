using CoreHub.Application.Features.Clients.Commands;
using CoreHub.Application.Features.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreHub.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetClients([FromQuery] GetClientsQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClient(Guid id)
    {
        var result = await _mediator.Send(new GetClientByIdQuery { Id = id });
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost]
    [Authorize(Policy = "CanViewOwnCases")]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess 
            ? CreatedAtAction(nameof(GetClient), new { id = result.Data }, result.Data)
            : BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "CanConfigureSystem")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var result = await _mediator.Send(new DeleteClientCommand { Id = id });
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpPost("{id}/close")]
    public async Task<IActionResult> CloseClient(Guid id, [FromBody] CloseClientCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{id}/dq")]
    public async Task<IActionResult> GetDataQuality(Guid id)
    {
        var result = await _mediator.Send(new GetClientDataQualityQuery { ClientId = id });
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }
}
