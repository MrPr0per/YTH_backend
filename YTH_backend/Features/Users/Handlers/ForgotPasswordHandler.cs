using MediatR;
using Microsoft.EntityFrameworkCore;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Email;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.Users.Handlers;

public class ForgotPasswordHandler(AppDbContext dbContext, JwtSettings jwtSettings, IConfiguration configuration, IEmailService emailService) : IRequestHandler<ForgotPasswordCommand>
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
        
        var registrationUrl = configuration["RegistrationUrl"]
                              ?? throw new InvalidOperationException("RegistrationUrl is not configured");
        
        var token = JwtHelper.GenerateVerificationToken(contextData, jwtSettings.Secret);
        var verificationLink =
            $"{registrationUrl}?token={Uri.EscapeDataString(token)}";
        
        await emailService.SendEmailAsync(
            to: request.Email,
            subject: "Смена пароля",
            htmlBody: $"""
                           <p>Здравствуйте!</p>
                           <p>
                               Для смены пароля перейдите по ссылке:
                           </p>
                           <p>
                               <a href="{verificationLink}">
                                   Сменить пароль
                               </a>
                           </p>
                           <p>Если вы не запрашивали смену пароля — просто проигнорируйте это письмо.</p>
                       """
        );
    }
}