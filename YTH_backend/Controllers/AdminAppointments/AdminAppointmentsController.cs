using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.Features.AdminAppointments.Commands;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.AdminAppointments;

[ApiController]
[Route("api/v0/users/{id}")]
public class AdminAppointmentsController(IMediator mediator) : ControllerBase
{
    [HttpPut("roles/admin")]
    [Authorize(Roles = "superadmin")]
    public async Task<IActionResult> AddAdminController(Guid id)
    {
        try
        {
            var query = new AddAdminCommand(id);
            await mediator.Send(query);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("roles/admin")]
    [Authorize(Roles = "superadmin")]
    public async Task<IActionResult> RemoveAdminController(Guid id)
    {
        try
        {
            var query = new RemoveAdminCommand(id);
            await mediator.Send(query);
            
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}