using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.Registrations;

[ApiController]
[Route("api/v0/registrations/courses")]
public class RegistrationCoursesController(IMediator mediator) : ControllerBase
{
    //TODO
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
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> AddCourseToUserController(AddCourseToUserRequestDto addCourseToUserRequestDto)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var command = new AddCourseToUserCommand(addCourseToUserRequestDto.Id, addCourseToUserRequestDto.CourseId,
                currentUserId);
            var response = await mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetUserCourseByIdController), 
                new { id = response.Id }, 
                response
                );
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    [HttpGet("{registrationId}")]
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> GetUserCourseByIdController(Guid registrationId)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new  GetUserCourseByIdQuery(registrationId, currentUserId);
            var response = await mediator.Send(query);
            
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpDelete("{registrationId}")]
    [Authorize(Roles = "logged_in,student,admin,superadmin")]
    public async Task<IActionResult> DeleteCourseFromUserController(Guid registrationId)
    {
        try
        {
            var currentUserId = JwtHelper.GetUserIdFromUser(User);
            var query = new DeleteCourseFromUserCommand(registrationId, currentUserId);
            await mediator.Send(query);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
        
}