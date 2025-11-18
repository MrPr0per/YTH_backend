using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Users.Handlers;

public class GetAllUsersHandler(AppDbContext dbContext) : IRequestHandler<GetAllUsersQuery, PagedResult<GetAllUsersResponseDto>>
{
    public async Task<PagedResult<GetAllUsersResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Users
            .ApplyOrderSettings(request.OrderType, request.OrderFieldName)
            .ApplyCursorSettings(request.CursorType, request.Take, request.CursorId);

        var data = await query
            .Select(u => new GetAllUsersResponseDto(
                u.Id,
                u.UserName,
                u.Email,
                u.Role))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetAllUsersResponseDto>(
            request.Take,
            request.OrderFieldName,
            request.OrderType,
            request.CursorType,
            data);
    }
}