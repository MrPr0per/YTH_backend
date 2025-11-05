using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Commands;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> LoginController([FromBody] LoginUserRequestDto loginUserRequestDto)
    {
        var command = new LoginUserCommand(loginUserRequestDto.Login, loginUserRequestDto.Password);
        throw new NotImplementedException();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> CreateUserController([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        //var command = new CreateUserCommand(createUserRequestDto.UserName, createUserRequestDto.Password, createUserRequestDto.Email);
        throw new NotImplementedException();
    }

    [HttpPost("sendVerificationEmailForRegistration")]
    public async Task<IActionResult> SendVerificationEmailController(
        [FromBody] SendVerificationEmailRequestDto sendVerificationEmailRequestDto)
    {
        var command = new SendVerificationEmailCommand(sendVerificationEmailRequestDto.Email);
        throw new NotImplementedException();
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutController()
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordController([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
    {
        //var command = new ChangePasswordCommand(User.FindFirst("UserId")?.Value, changePasswordRequestDto.NewPassword, changePasswordRequestDto.OldPassword);
        throw new NotImplementedException();
    }
    
    [HttpPost("forgotPassword ")]
    public async Task<IActionResult> ForgotPasswordController([FromBody] ForgotPasswordRequestDto forgotPasswordRequestDto)
    {
        var command = new ForgotPasswordCommand(forgotPasswordRequestDto.Email);
        throw new NotImplementedException();
    }
    
    [Authorize]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPasswordController([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
    {
        //TODO надо разобряться с Jwt
        
        //var command = new ResetPasswordCommand(resetPasswordRequestDto.NewPassword);
        throw new NotImplementedException();
    }
}