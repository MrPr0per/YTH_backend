using MediatR;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;

namespace YTH_backend.Controllers.Registrations;

[ApiController]
[Route("api/v0/registrations")]
public class RegistrationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserCoursesController(Guid id, [FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        var orderParams = QueryParamsParser.ParseOrderParams(order);
        var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
        
        if (take <= 0)
            take = 10;
        
        var query = new GetUserCoursesQuery(id, take, orderParams.OrderType, cursorParams.CursorType, cursorParams.CursorId, orderParams.FieldName);
        await mediator.Send(query);
        throw new NotImplementedException();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCourseToUserController(AddCourseToUserRequestDto addCourseToUserRequestDto)
    {
        var command = new AddCourseToUserCommand(addCourseToUserRequestDto.Id, addCourseToUserRequestDto.CourseId);
        throw new NotImplementedException();
    }
}