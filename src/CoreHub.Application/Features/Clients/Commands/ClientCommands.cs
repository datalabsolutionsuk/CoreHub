using CoreHub.Application.Common;
using CoreHub.Application.DTOs;
using MediatR;

namespace CoreHub.Application.Features.Clients.Commands;

public record CreateClientCommand : IRequest<Result<Guid>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public int Gender { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public string? PostalCode { get; init; }
    public Guid? AssignedPractitionerId { get; init; }
    public Guid? ProgramId { get; init; }
    public Guid? SubsiteId { get; init; }
    public DateTime? ReferralDate { get; init; }
}

public record UpdateClientCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public int Gender { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public string? PostalCode { get; init; }
    public Guid? AssignedPractitionerId { get; init; }
}

public record CloseClientCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public int DischargeReason { get; init; }
    public string? Notes { get; init; }
}

public record DeleteClientCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
}
