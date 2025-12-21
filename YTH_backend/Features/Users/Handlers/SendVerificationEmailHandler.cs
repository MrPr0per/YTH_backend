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

public class SendVerificationEmailHandler(AppDbContext dbContext, IEmailService emailService, JwtSettings jwtSettings, IConfiguration configuration) : IRequestHandler<SendVerificationEmailCommand>
{
    public async Task Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        if (!EmailHelper.IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email");
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user != null)
            throw new EntityAlreadyExistsException($"User with email:{request.Email} is already exists");
        
        var token = JwtHelper.GenerateVerificationToken(new Dictionary<string, object>{["email"] = request.Email, ["id"] = Guid.NewGuid()}, jwtSettings.Secret);
        
        var registrationUrl = configuration["RegistrationUrl"]
                              ?? throw new InvalidOperationException("RegistrationUrl is not configured");
        
        var verificationLink =
            $"{registrationUrl}?token={Uri.EscapeDataString(token)}";

        await emailService.SendEmailAsync(
            to: request.Email,
            subject: "Подтверждение регистрации",
            htmlBody: $"""
                           <p>Здравствуйте!</p>
                           <p>
                               Для завершения регистрации перейдите по ссылке:
                           </p>
                           <p>
                               <a href="{verificationLink}">
                                   Подтвердить регистрацию
                               </a>
                           </p>
                           <p>Если вы не запрашивали регистрацию — просто проигнорируйте это письмо.</p>
                       """
        );
    }
}