using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;

namespace YTH_backend.Controllers.Courses;

[ApiController]
[Route("api/v0/courses")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseController(Guid id)
    {
        var query = new GetCourseQuery(id);
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCoursesController([FromQuery] int from = 0, [FromQuery] int take = 10, [FromQuery] OrderType orderType = OrderType.Asc)
    {
        var query = new GetAllCoursesQuery(from, take, orderType);
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddCourseController([FromBody] AddCourseRequestDto addCourseRequestDto)
    {
        var command = new AddCourseCommand(addCourseRequestDto.Name, addCourseRequestDto.Description, addCourseRequestDto.Link);
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourseController(Guid id)
    {
        var query = new DeleteCourseCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchCourseController(Guid id, [FromBody] JsonPatchDocument<PatchCourseRequestDto> patchCourseRequestDto)
    {
        var command = new PatchCourseCommand(id, patchCourseRequestDto);
        throw new NotImplementedException();
    }
}