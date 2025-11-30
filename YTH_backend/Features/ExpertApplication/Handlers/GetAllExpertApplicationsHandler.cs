using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class GetAllExpertApplicationsHandler(AppDbContext dbContext)
    : IRequestHandler<GetAllExpertApplicationsQuery, PagedResult<GetExpertApplicationResponseDto>>
{
    public async Task<PagedResult<GetExpertApplicationResponseDto>> Handle(GetAllExpertApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        if (request is { IsAdmin: false, CreatedBy: not null } && request.CurrentUserId != request.CreatedBy ||
            request is { IsAdmin: true, Status: ExpertApplicationStatus.Created })
            throw new UnauthorizedAccessException();

        var query = dbContext.ExpertApplications.AsQueryable();

        if (request.CreatedBy != null)
            query = query.Where(x => x.UserId == request.CreatedBy);

        if (request.Status != null)
            query = query.Where(x => x.Status == request.Status);

        if (request.AcceptedBy != null)
            query = query.Where(x => x.AcceptedBy == request.AcceptedBy);

        var take = request.Take;

        if (take < 0)
            take = 10;

        query = query
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);

        var data = await query
            .Select(ea => new GetExpertApplicationResponseDto(
                ea.Id,
                ea.UserId,
                ea.Message,
                ea.Status,
                ea.AcceptedBy,
                ea.IsApproved,
                ea.ResolutionMessage))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetExpertApplicationResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data
        );
    }
}