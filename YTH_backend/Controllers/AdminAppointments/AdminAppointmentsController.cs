using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.Features.AdminAppointments.Commands;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.AdminAppointments;

[ApiController]
[Route("api/v0/users/{id:guid}/roles/admin")]
public class AdminAppointmentsController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    [Authorize(Roles = "superadmin")]
    public async Task<IActionResult> AddAdminController([FromRoute] Guid id)
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

    [HttpDelete]
    [Authorize(Roles = "superadmin")]
    public async Task<IActionResult> RemoveAdminController([FromRoute] Guid id)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new RemoveAdminCommand(id, currentUserId);
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
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}