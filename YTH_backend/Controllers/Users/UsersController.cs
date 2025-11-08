using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.DTOs.Event;
using YTH_backend.DTOs.User;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Features.Users.Commands;
using YTH_backend.Features.Users.Queries;
using YTH_backend.Models;

namespace YTH_backend.Controllers.Users;

[ApiController]
[Route("api/v0/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPersonalDataController()
    {
        var query = new GetPersonalDataQuery();
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserController(Guid id)
    {
        var query = new GetUserQuery(id);
        throw new NotImplementedException();
    }

    [HttpGet("{id}/events")]
    public async Task<IActionResult> GetUserEventsController(Guid id, [FromQuery] int from = 0, [FromQuery] int take = 10, [FromQuery] OrderType orderType = OrderType.Asc)
    {
        var query = new GetUserEventsQuery(id, from, take, orderType);
        throw new NotImplementedException();
    }

    [HttpPost("{id}/anonymize")]
    public async Task<IActionResult> AnonimyzeUserController(Guid id)
    {
        var command = new AnonymizeUserCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPost("{id}/events/{eventId}")]
    public async Task<IActionResult> AddEventsToUserController(Guid id, Guid eventId)
    {
        var command = new AddEventToUserCommand(id, eventId);
        throw new NotImplementedException();
    }

    [HttpDelete("{id}/events/{eventId}")]
    public async Task<IActionResult> DeleteEventFromUserController(Guid id, Guid eventId)
    {
        var command = new DeleteEventFromUserCommand(id, eventId);
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id}/courses/{eventId}")]
    public async Task<IActionResult> DeleteCourseFromUserController(Guid id, Guid eventId)
    {
        var command = new DeleteCourseFromUserCommand(id, eventId);
        throw new NotImplementedException();
    }
    
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> PatchUserController(Guid id,
        [FromBody] JsonPatchDocument<PatchUserRequestDto> patchUserRequestDto)
    {
        throw new NotImplementedException();
    }
}