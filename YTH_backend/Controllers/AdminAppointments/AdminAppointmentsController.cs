using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.Features.AdminAppointments.Commands;

namespace YTH_backend.Controllers.AdminAppointments;

[ApiController]
[Route("api/v0/users/{id}")]
public class AdminAppointmentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPut("roles/admin")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AddAdminController(Guid id)
    {
        var query = new AddAdminCommand(id);
        throw new NotImplementedException();
    }

    [HttpDelete("roles/admin")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> RemoveAdminController(Guid id)
    {
        var query = new RemoveAdminCommand(id);
        throw new NotImplementedException();
    }
}