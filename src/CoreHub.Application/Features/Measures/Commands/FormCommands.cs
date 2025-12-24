using CoreHub.Application.Common;
using CoreHub.Application.DTOs;
using MediatR;

namespace CoreHub.Application.Features.Measures.Commands;

public record CreateFormCommand : IRequest<Result<Guid>>
{
    public Guid ClientId { get; init; }
    public Guid MeasureId { get; init; }
    public Guid? SessionId { get; init; }
    public int Mode { get; init; }
    public bool GenerateRemoteLink { get; init; }
}

public record SubmitFormCommand : IRequest<Result<bool>>
{
    public Guid FormId { get; init; }
    public string? AccessToken { get; init; }
    public Dictionary<Guid, int> Answers { get; init; } = new();
}

public record GenerateFormLinkCommand : IRequest<Result<FormLinkDto>>
{
    public Guid FormId { get; init; }
    public int ExpiryHours { get; init; } = 72;
}
