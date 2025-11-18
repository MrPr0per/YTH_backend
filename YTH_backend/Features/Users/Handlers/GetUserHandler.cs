using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Posts.Queries;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Features.Users.Handlers;

public class GetUserHandler(AppDbContext dbContext) : IRequestHandler<GetUserQuery, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.Id], cancellationToken);

        if (user != null)
            return new GetUserResponseDto(user.UserName, user.Email, user.Role);
        
        throw new EntityNotFoundException($"User with id:{request.Id} not found");
    }
}