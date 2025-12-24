using CoreHub.Application.Common;
using CoreHub.Application.DTOs;
using CoreHub.Application.Features.Clients.Queries;
using CoreHub.Application.Interfaces;
using CoreHub.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace CoreHub.Application.Features.Clients.Handlers;

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, Result<PagedResult<ClientDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetClientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagedResult<ClientDto>>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.Clients
                .Include(c => c.AssignedPractitioner)
                .Include(c => c.Program)
                .Include(c => c.Flags.Where(f => !f.IsCleared))
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(request.SearchTerm) ||
                    c.LastName.Contains(request.SearchTerm) ||
                    c.ClientCode.Contains(request.SearchTerm));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(c => (int)c.Status == request.Status.Value);
            }

            if (request.PractitionerId.HasValue)
            {
                query = query.Where(c => c.AssignedPractitionerId == request.PractitionerId.Value);
            }

            if (request.ProgramId.HasValue)
            {
                query = query.Where(c => c.ProgramId == request.ProgramId.Value);
            }

            if (request.HasActiveFlags.HasValue)
            {
                query = request.HasActiveFlags.Value
                    ? query.Where(c => c.Flags.Any(f => !f.IsCleared))
                    : query.Where(c => !c.Flags.Any(f => !f.IsCleared));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var clients = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = clients.Select(c => new ClientDto
            {
                Id = c.Id,
                ClientCode = c.ClientCode,
                FirstName = c.FirstName,
                LastName = c.LastName,
                FullName = c.FullName,
                DateOfBirth = c.DateOfBirth,
                Gender = c.Gender,
                Email = c.Email,
                Phone = c.Phone,
                Status = c.Status,
                AssignedPractitionerId = c.AssignedPractitionerId,
                AssignedPractitionerName = c.AssignedPractitioner?.FullName,
                ProgramId = c.ProgramId,
                ProgramName = c.Program?.Name,
                DataQualityScore = c.DataQualityScore,
                CreatedAt = c.CreatedAt,
                ClosedDate = c.ClosedDate,
                ActiveFlags = c.Flags.Where(f => !f.IsCleared).Select(f => f.Type.ToString()).ToList()
            }).ToList();

            var result = new PagedResult<ClientDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return Result<PagedResult<ClientDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<ClientDto>>.Failure($"Error retrieving clients: {ex.Message}");
        }
    }
}

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<ClientDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetClientByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ClientDetailDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var client = await _context.Clients
                .Include(c => c.AssignedPractitioner)
                .Include(c => c.Program)
                .Include(c => c.Subsite)
                .Include(c => c.Flags.Where(f => !f.IsCleared))
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (client == null)
                return Result<ClientDetailDto>.Failure("Client not found");

            var dto = new ClientDetailDto
            {
                Id = client.Id,
                ClientCode = client.ClientCode,
                FirstName = client.FirstName,
                LastName = client.LastName,
                FullName = client.FullName,
                DateOfBirth = client.DateOfBirth,
                Gender = client.Gender,
                Email = client.Email,
                Phone = client.Phone,
                Address = client.Address,
                PostalCode = client.PostalCode,
                Status = client.Status,
                AssignedPractitionerId = client.AssignedPractitionerId,
                AssignedPractitionerName = client.AssignedPractitioner?.FullName,
                ProgramId = client.ProgramId,
                ProgramName = client.Program?.Name,
                SubsiteId = client.SubsiteId,
                SubsiteName = client.Subsite?.Name,
                DataQualityScore = client.DataQualityScore,
                ReferralDate = client.ReferralDate,
                FirstAppointmentDate = client.FirstAppointmentDate,
                CreatedAt = client.CreatedAt,
                ClosedDate = client.ClosedDate,
                DischargeReason = client.DischargeReason,
                DischargeNotes = client.DischargeNotes,
                ConsentToContact = client.ConsentToContact,
                ConsentToResearch = client.ConsentToResearch,
                ActiveFlags = client.Flags.Where(f => !f.IsCleared).Select(f => f.Type.ToString()).ToList()
            };

            return Result<ClientDetailDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<ClientDetailDto>.Failure($"Error retrieving client: {ex.Message}");
        }
    }
}
