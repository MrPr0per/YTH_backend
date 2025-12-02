using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Features.Users.Handlers;

public class LogoutHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        var refreshToken = context.Request.Cookies["refreshToken"] 
                           ?? context.Request.Headers["Refresh-Token"].ToString();

        if (!string.IsNullOrEmpty(refreshToken))
        {
            var token = await dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == refreshToken, cancellationToken);
            
            if (token != null)
            {
                dbContext.RefreshTokens.Remove(token);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        context.Response.Cookies.Delete("refreshToken");
    }
}