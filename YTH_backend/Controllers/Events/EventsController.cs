using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure;

namespace YTH_backend.Controllers.Events;

[ApiController]
[Route("api/v0/events")]
public class EventsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEventController(Guid id)
    {
        var query = new GetEventQuery(id);
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEventsController([FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        var orderParams = QueryParamsParser.ParseOrderParams(order);
        var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
        
        if (take <= 0)
            take = 10;
        
        var query = new GetAllEventsQuery(take, orderParams.OrderType, cursorParams.CursorType, orderParams.FieldName, cursorParams.CursorId);
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