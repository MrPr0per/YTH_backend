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

public class SendVerificationEmailHandler(AppDbContext dbContext, IEmailService emailService, JwtSettings jwtSettings) : IRequestHandler<SendVerificationEmailCommand>
{
    public async Task Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        if (!EmailHelper.IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email");
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user != null)
            throw new EntityAlreadyExistsException($"User with email:{request.Email} is already exists");
        
        var token = JwtHelper.GenerateVerificationToken(new Dictionary<string, object>{["email"] = request.Email, ["id"] = Guid.NewGuid()}, jwtSettings.Secret);
        Console.WriteLine(token);
        //TODO
        //var verificationLink = $"{registrationUrl}?token={token}";
        // await emailService.SendEmailAsync(
        //     request.Email,
        //     "Подтверждение регистрации",
        //     $"<p>Перейдите по ссылке для завершения регистрации: <a href='{verificationLink}'>{verificationLink}</a></p>"
        // );
    }
}