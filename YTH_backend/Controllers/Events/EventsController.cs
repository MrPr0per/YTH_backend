using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;
using YTH_backend.Features.Events.Commands;
using YTH_backend.Features.Events.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Events;

[ApiController]
[Route("api/v0/events")]
public class EventsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}", Name = nameof(GetEventController))]
    public async Task<IActionResult> GetEventController(Guid id)
    {
        try
        {
            var query = new GetEventQuery(id);
            var response = await mediator.Send(query);
                
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(new {error = e.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllEventsController([FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            var query = new GetAllEventsQuery(take, orderParams.OrderType, cursorParams.CursorType,
                orderParams.FieldName, cursorParams.CursorId);
            var response = await mediator.Send(query);
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddEventController([FromBody] AddEventRequestDto addEventRequestDto)
    {
        var command = new AddEventCommand(addEventRequestDto.Name, addEventRequestDto.Description, addEventRequestDto.Type, addEventRequestDto.Date, addEventRequestDto.Address);
        var response = await mediator.Send(command);
        
        return CreatedAtAction(
            nameof(GetEventController),  
            new { id = response.Id },     
            response                       
        );
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteEventController(Guid id)
    {
        try
        {
            var command = new DeleteEventCommand(id);
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(new { error = e.Message });
        }
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> PatchEventController(Guid id, [FromBody] JsonPatchDocument<PatchEventRequestDto> patchEventRequestDto)
    {
        try
        {
            var command = new PatchEventCommand(id, patchEventRequestDto);
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(new { error = e.Message });
        }
    }
}