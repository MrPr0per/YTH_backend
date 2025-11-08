using MediatR;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;

namespace YTH_backend.Controllers.Registrations;

[ApiController]
[Route("api/v0/registrations")]
public class RegistrationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;
    
    [HttpGet]
    public async Task<IActionResult> GetUserCoursesController(Guid id, [FromQuery] int from = 0, [FromQuery] int take = 10, [FromQuery] OrderType orderType = OrderType.Asc)
    {
        var query = new GetUserCoursesQuery(id, from, take, orderType);
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