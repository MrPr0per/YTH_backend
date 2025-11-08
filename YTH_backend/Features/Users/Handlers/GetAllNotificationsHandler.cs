using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Models;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class GetAllNotificationsHandler(AppDbContext context) : IRequestHandler<GetAllNotificationsQuery, PagedResult<GetNotificationsResponseDto>>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task<PagedResult<GetNotificationsResponseDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrentUserId != request.Id)
            throw new AccessViolationException("User does not have permission to view other users notifications");
                
        IQueryable<Notification> query = dbContext.Notifications;
        
        query = request.OrderType == OrderType.Asc
            ? query.OrderBy(x => x.CreatedAt)
            : query.OrderByDescending(x => x.CreatedAt);
        
        var from = request.From < 1 ? 1 : request.From;
        var skip = from - 1;
        
        var data = await query
            .Skip(skip)
            .Take(request.Take)
            .Select(n => new GetNotificationsResponseDto(
                n.Title,
                n.NotificationText,
                n.CreatedAt,
                n.IsRead))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetNotificationsResponseDto>(
            request.From,
            request.Take,
            request.OrderType,
            data);
    }
}