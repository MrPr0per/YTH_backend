using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Features.Events.Queries;

namespace YTH_backend.Controllers.Events;

[ApiController]
[Route("api/v0/events")]
public class EventsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEventController(Guid id)
    {
        var query = new GetEventQuery(id);
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEventsController([FromQuery] int from = 0, [FromQuery] int take = 10, [FromQuery] OrderType orderType = OrderType.Asc)
    {
        var query = new GetAllEventsQuery(from, take, orderType);
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> AddEventController([FromBody] AddEventRequestDto addEventRequestDto)
    {
        var command = new AddEventCommand(addEventRequestDto.Name, addEventRequestDto.Description, addEventRequestDto.ShortDescription, addEventRequestDto.Type, addEventRequestDto.Date, addEventRequestDto.Address);
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> DeleteEventController(Guid id)
    {
        var command = new DeleteEventCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> PatchEventController(Guid id, [FromBody] JsonPatchDocument<PatchEventRequestDto> patchEventRequestDto)
    {
        var command = new PatchEventCommand(id, patchEventRequestDto);
        throw new NotImplementedException();
    }
}