using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Features.ExpertApplication.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.ExpertApplication.Handlers;

public class GetApplicationHistoryHandler(AppDbContext dbContext) : IRequestHandler<GetApplicationHistoryQuery, PagedResult<GetExpertApplicationActionResponseDto>>
{
    public async Task<PagedResult<GetExpertApplicationActionResponseDto>> Handle(GetApplicationHistoryQuery request, CancellationToken cancellationToken)
    {
        var application = await dbContext.ExpertApplications.FindAsync([request.ApplicationId], cancellationToken);
        
        if (application == null)
            throw new EntityNotFoundException($"Expert application with id: {request.ApplicationId} does not exist");

        if (application.UserId != request.CurrentUserId)
            throw new UnauthorizedAccessException();
        
        var query = dbContext.ExpertApplicationActions
            .Where(a => a.ExpertApplicationId == request.ApplicationId)
            .AsQueryable();
        
        var take = request.Take;

        if (take < 0)
            take = 10;
        
        query = query
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, take, request.CursorId);

        var data = await query
            .Select(a => new GetExpertApplicationActionResponseDto(
                a.Id,
                a.ExpertApplicationId,
                a.CreatedAt,
                a.ExpertApplicationActionType,
                a.Other))
            .ToListAsync(cancellationToken);
        
        return new PagedResult<GetExpertApplicationActionResponseDto>(
            take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}