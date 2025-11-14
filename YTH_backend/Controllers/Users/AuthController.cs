using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginController([FromBody] LoginUserRequestDto loginUserRequestDto)
    {
        var command = new LoginUserCommand(loginUserRequestDto.Login, loginUserRequestDto.Password);
        throw new NotImplementedException();
    }
    
    [HttpPost("register")]
    [Authorize(Roles = "with_confirmed_email")]
    public async Task<IActionResult> CreateUserController([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        var email = User.FindFirstValue("email");
        
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required to register a user.");

        try
        {
            var command = new CreateUserCommand(createUserRequestDto.UserName, createUserRequestDto.Password, email);
            var createUserResponseDto = await mediator.Send(command);
            
            Response.Cookies.Append("refreshToken",  createUserResponseDto.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });
            
            return Created("", new {access_token = createUserResponseDto.AccessToken});
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("sendVerificationEmailForRegistration")]
    public async Task<IActionResult> SendVerificationEmailController(
        [FromBody] SendVerificationEmailRequestDto sendVerificationEmailRequestDto)
    {
        var command = new SendVerificationEmailCommand(sendVerificationEmailRequestDto.Email);
        try
        {
            await mediator.Send(command);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize("logged_in,student,admin,superadmin")]
    public async Task<IActionResult> LogoutController()
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("changePassword")]
    [Authorize("logged_in,student,admin,superadmin")]
    public async Task<IActionResult> ChangePasswordController([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
    {
        //var command = new ChangePasswordCommand(User.FindFirst("UserId")?.Value, changePasswordRequestDto.NewPassword, changePasswordRequestDto.OldPassword);
        throw new NotImplementedException();
    }
    
    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPasswordController([FromBody] ForgotPasswordRequestDto forgotPasswordRequestDto)
    {
        var command = new ForgotPasswordCommand(forgotPasswordRequestDto.Email);
        throw new NotImplementedException();
    }
    
    [Authorize]
    [HttpPost("resetPassword")]
    [Authorize(Roles = "with_confirmed_email")]
    public async Task<IActionResult> ResetPasswordController([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
    {
        //TODO надо разобряться с Jwt
        
        //var command = new ResetPasswordCommand(resetPasswordRequestDto.NewPassword);
        throw new NotImplementedException();
    }
}