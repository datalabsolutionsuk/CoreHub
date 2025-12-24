using CoreHub.Application.Common;
using CoreHub.Application.DTOs;
using MediatR;

namespace CoreHub.Application.Features.Measures.Queries;

public record GetMeasuresQuery : IRequest<Result<List<MeasureDto>>>
{
    public bool? IsActive { get; init; } = true;
}

public record GetMeasureDetailQuery : IRequest<Result<MeasureDetailDto>>
{
    public Guid MeasureId { get; init; }
}

public record GetClientFormsQuery : IRequest<Result<List<AdministeredFormDto>>>
{
    public Guid ClientId { get; init; }
    public Guid? MeasureId { get; init; }
}

public record GetFormByTokenQuery : IRequest<Result<FormForCompletionDto>>
{
    public string Token { get; init; } = string.Empty;
}

public record FormForCompletionDto
{
    public Guid FormId { get; init; }
    public string ClientFirstName { get; init; } = string.Empty;
    public MeasureDetailDto Measure { get; init; } = null!;
    public bool IsExpired { get; init; }
    public bool IsCompleted { get; init; }
}
