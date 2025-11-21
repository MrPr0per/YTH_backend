using MediatR;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Registrations;

[ApiController]
[Route("api/v0/registrations")]
public class RegistrationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserCoursesController(Guid id, [FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
            
            var query = new GetUserCoursesQuery(id, take, orderParams.OrderType, cursorParams.CursorType,
                cursorParams.CursorId, orderParams.FieldName);
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
    public async Task<IActionResult> AddCourseToUserController(AddCourseToUserRequestDto addCourseToUserRequestDto)
    {
        var command = new AddCourseToUserCommand(addCourseToUserRequestDto.Id, addCourseToUserRequestDto.CourseId);
        throw new NotImplementedException();
    }
}