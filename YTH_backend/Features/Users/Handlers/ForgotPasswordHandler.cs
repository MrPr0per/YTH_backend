using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Email;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Users.Handlers;

public class ForgotPasswordHandler(AppDbContext dbContext, JwtSettings jwtSettings) : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        if (!EmailHelper.IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email");
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null)
            throw new EntityNotFoundException($"User with email:{request.Email} doesn't exists");
        
        
        var contextData = new Dictionary<string, object>
        {
            ["email"] = user.Email,
            ["id"] = user.Id
        };
        
        var token = JwtHelper.GenerateVerificationToken(contextData, jwtSettings.Secret);
        //TODO
        //var verificationLink = $"{registrationUrl}?token={token}";
        // await emailService.SendEmailAsync(
        //     request.Email,
        //     "Смена пароля",
        //     $"<p>Перейдите по ссылке для смены пароля: <a href='{verificationLink}'>{verificationLink}</a></p>"
        // );
    }
}