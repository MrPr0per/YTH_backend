using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginController([FromBody] LoginUserRequestDto loginUserRequestDto)
    {
        try
        {
            var command = new LoginUserCommand(loginUserRequestDto.Login, loginUserRequestDto.Password);
            var response = await mediator.Send(command);
            Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            return Ok(new { access_token = response.AccessToken });
        }
        catch (EntityNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (TooManyRequestsException ex)
        {
            return StatusCode(429, new { error = ex.Message });
        }
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
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshTokenController()
    {
        try
        {
            var response = await mediator.Send(new RefreshCommand());

            Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            return Ok(new { access_token = response.AccessToken });
        }
        catch (TokenExpiredException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (TooManyRequestsException ex)
        {
            return StatusCode(429, new { error = ex.Message });
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
    }

    [HttpPost("logout")]
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> LogoutController()
    {
    
        await mediator.Send(new LogoutCommand());
        return NoContent();
    }
    
    [HttpPost("changePassword")]
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> ChangePasswordController([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new ChangePasswordCommand(userId, changePasswordRequestDto.NewPassword,
                changePasswordRequestDto.OldPassword);
            
            await mediator.Send(command);
            
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("sendVerificationEmailForResetPassword")]
    public async Task<IActionResult> ForgotPasswordController([FromBody] ForgotPasswordRequestDto forgotPasswordRequestDto)
    {
        try
        {
            var command = new ForgotPasswordCommand(forgotPasswordRequestDto.Email);
            await mediator.Send(command);
            
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    
    [HttpPost("resetPassword")]
    [Authorize(Roles = "with_confirmed_email")]
    public async Task<IActionResult> ResetPasswordController([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new ResetPasswordCommand(userId, resetPasswordRequestDto.NewPassword);
            await mediator.Send(command);
            
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}