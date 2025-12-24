using CoreHub.Application.Common;
using CoreHub.Application.DTOs;
using MediatR;

namespace CoreHub.Application.Features.Clients.Queries;

public record GetClientsQuery : IRequest<Result<PagedResult<ClientDto>>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchTerm { get; init; }
    public int? Status { get; init; }
    public Guid? PractitionerId { get; init; }
    public Guid? ProgramId { get; init; }
    public bool? HasActiveFlags { get; init; }
}

public record GetClientByIdQuery : IRequest<Result<ClientDetailDto>>
{
    public Guid Id { get; init; }
}

public record GetClientDataQualityQuery : IRequest<Result<DataQualityDto>>
{
    public Guid ClientId { get; init; }
}

public record DataQualityDto
{
    public int Score { get; init; }
    public Dictionary<string, bool> RequiredFields { get; init; } = new();
    public List<string> MissingFields { get; init; } = new();
}
