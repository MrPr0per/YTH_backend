using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Courses;

[ApiController]
[Route("api/v0/courses")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}", Name = nameof(GetCourseController))]
    public async Task<IActionResult> GetCourseController([FromRoute] Guid id)
    {
        try
        {
            var query = new GetCourseQuery(id);
            var response = await mediator.Send(query);
            
            return Ok(response);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(new {error = e.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCoursesController([FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            var query = new GetAllCoursesQuery(take, orderParams.OrderType, cursorParams.CursorType,
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
    public async Task<IActionResult> AddCourseController([FromBody] AddCourseRequestDto addCourseRequestDto)
    {
        try
        {
            var command = new AddCourseCommand(addCourseRequestDto.Name, addCourseRequestDto.Description,
                addCourseRequestDto.Link, addCourseRequestDto.ImageBase64, addCourseRequestDto.Price);
            var response = await mediator.Send(command);

            return CreatedAtAction(
                nameof(GetCourseController),
                new { id = response.Id },
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteCourseController([FromRoute] Guid id)
    {
        try
        {
            var command = new DeleteCourseCommand(id);
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(new { error = e.Message });
        }
    }
    
    [HttpPatch("{id:guid}")]
    [Consumes("application/json-patch+json")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> PatchCourseController([FromRoute] Guid id, [FromBody] JsonPatchDocument<PatchCourseRequestDto> patchCourseRequestDto)
    {
        try
        {
            var command = new PatchCourseCommand(id, patchCourseRequestDto);
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(new { error = e.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}