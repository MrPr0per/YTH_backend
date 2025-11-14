using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Email;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Users.Handlers;

public class SendVerificationEmailHandler(AppDbContext dbContext, IEmailService emailService, JwtSetting jwtSetting) : IRequestHandler<SendVerificationEmailCommand>
{
    public async Task Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        var token = JwtHelper.GenerateVerificationToken(request.Email, jwtSetting.Secret);
        if (!EmailHelper.IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email");
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user != null)
            throw new EntityAlreadyExistsException($"Email:{request.Email} is already exists");
        //TODO
        //var verificationLink = $"{registrationUrl}?token={token}";
        // await emailService.SendEmailAsync(
        //     request.Email,
        //     "Подтверждение регистрации",
        //     $"<p>Перейдите по ссылке для завершения регистрации: <a href='{verificationLink}'>{verificationLink}</a></p>"
        // );
    }
}