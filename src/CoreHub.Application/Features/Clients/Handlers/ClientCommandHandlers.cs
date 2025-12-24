using CoreHub.Application.Common;
using CoreHub.Application.Features.Clients.Commands;
using CoreHub.Application.Interfaces;
using CoreHub.Domain.Entities;
using CoreHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreHub.Application.Features.Clients.Handlers;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateClientCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Generate unique client code
            var clientCode = await GenerateClientCodeAsync();

            var client = new Client
            {
                Id = Guid.NewGuid(),
                TenantId = _currentUser.TenantId ?? Guid.Empty,
                ClientCode = clientCode,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = (Gender)request.Gender,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address,
                PostalCode = request.PostalCode,
                AssignedPractitionerId = request.AssignedPractitionerId,
                ProgramId = request.ProgramId,
                SubsiteId = request.SubsiteId,
                ReferralDate = request.ReferralDate ?? DateTime.UtcNow,
                Status = ClientStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(client.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Error creating client: {ex.Message}");
        }
    }

    private async Task<string> GenerateClientCodeAsync()
    {
        // Generate sequential client code (C000001, C000002, etc.)
        var lastClient = await _context.Clients
            .OrderByDescending(c => c.ClientCode)
            .FirstOrDefaultAsync();

        if (lastClient == null)
            return "C000001";

        // Extract number from last client code and increment
        if (int.TryParse(lastClient.ClientCode[1..], out var lastNumber))
        {
            var nextNumber = lastNumber + 1;
            return $"C{nextNumber:D6}";
        }

        return "C000001";
    }
}

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var client = await _context.Clients.FindAsync(new object[] { request.Id }, cancellationToken);

            if (client == null)
                return Result<bool>.Failure("Client not found");

            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.DateOfBirth = request.DateOfBirth;
            client.Gender = (Gender)request.Gender;
            client.Email = request.Email;
            client.Phone = request.Phone;
            client.Address = request.Address;
            client.PostalCode = request.PostalCode;
            client.AssignedPractitionerId = request.AssignedPractitionerId;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error updating client: {ex.Message}");
        }
    }
}

public class CloseClientCommandHandler : IRequestHandler<CloseClientCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public CloseClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(CloseClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var client = await _context.Clients.FindAsync(new object[] { request.Id }, cancellationToken);

            if (client == null)
                return Result<bool>.Failure("Client not found");

            client.Status = ClientStatus.Discharged;
            client.DischargeReason = (DischargeReason)request.DischargeReason;
            client.DischargeNotes = request.Notes;
            client.ClosedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error closing client: {ex.Message}");
        }
    }
}
