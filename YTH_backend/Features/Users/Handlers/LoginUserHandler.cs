using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using YTH_backend.Data;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Models.User;

namespace YTH_backend.Features.Users.Handlers;

public class LoginUserHandler(AppDbContext context) : IRequestHandler<LoginUserCommand>
{
    private readonly AppDbContext dbContext = context;
    
    public async Task Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // private string GenerateToken(User user)
    // {
    //     var claims = new List<Claim>
    //     {
    //         new Claim("UserId", user.Id.ToString()),
    //         new Claim(ClaimTypes.Name, user.UserName),
    //         new Claim(ClaimTypes.Email, user.Email),
    //         new Claim(ClaimTypes.Role, user.Role.ToString())
    //     };
    //     
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //
    //     var token = new JwtSecurityToken(
    //         issuer: _issuer,
    //         audience: _audience,
    //         claims: claims,
    //         expires: DateTime.UtcNow.AddMinutes(60),
    //         signingCredentials: creds
    //     );
    //
    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }
}