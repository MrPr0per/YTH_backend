using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.Course;
using YTH_backend.Enums;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Features.Courses.Queries;
using YTH_backend.Infrastructure;

namespace YTH_backend.Controllers.Courses;

[ApiController]
[Route("api/v0/courses")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCourseController(Guid id)
    {
        var query = new GetCourseQuery(id);
        throw new NotImplementedException();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCoursesController([FromQuery] string? cursor = null, [FromQuery] int take = 10, [FromQuery] string? order = null)
    {
        var orderParams = QueryParamsParser.ParseOrderParams(order);
        var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
        
        if (take <= 0)
            take = 10;
        
        var query = new GetAllCoursesQuery(take, orderParams.OrderType, cursorParams.CursorType, orderParams.FieldName, cursorParams.CursorId);
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> AddCourseController([FromBody] AddCourseRequestDto addCourseRequestDto)
    {
        var command = new AddCourseCommand(addCourseRequestDto.Name, addCourseRequestDto.Description, addCourseRequestDto.Link);
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> DeleteCourseController(Guid id)
    {
        var query = new DeleteCourseCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> PatchCourseController(Guid id, [FromBody] JsonPatchDocument<PatchCourseRequestDto> patchCourseRequestDto)
    {
        var command = new PatchCourseCommand(id, patchCourseRequestDto);
        throw new NotImplementedException();
    }
}