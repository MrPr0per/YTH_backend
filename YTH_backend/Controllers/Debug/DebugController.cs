using MediatR;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.Features.Debug;

namespace YTH_backend.Controllers.Debug;

[Route("api/v0/debug")]
public class DebugController(IMediator mediator) : ControllerBase
{
    [HttpPost("addUser")]
    public async Task<IActionResult> AddUserController([FromBody] AddUserDebugDto request)
    {
        var command = new AddUserDebugCommand(request.Username, request.Password, request.Email, request.Role);
        var token = await mediator.Send(command);
        return Ok(token);
    }
}