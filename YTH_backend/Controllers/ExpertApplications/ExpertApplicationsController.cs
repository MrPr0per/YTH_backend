using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Enums;
using YTH_backend.Features.ExpertApplication.Commands;
using YTH_backend.Features.ExpertApplication.Queries;
using YTH_backend.Infrastructure;
using YTH_backend.Infrastructure.Exceptions;

namespace YTH_backend.Controllers.ExpertApplications;

[ApiController]
[Route("api/v0/expertApplications")]
public class ExpertApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> CreateExpertApplicationController(
        [FromBody] CreateExpertApplicationRequestDto request)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            var command = new CreateExpertApplicationCommand(userId, request.Message);
            var response = await mediator.Send(command);

            return CreatedAtAction(nameof(GetExpertApplicationById),
                new { id = response.Id },
                response);
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> GetAllExpertApplications([FromQuery] string? cursor = null,
        [FromQuery] int take = 10,
        [FromQuery] string? order = null, [FromQuery] Guid? createdBy = null,
        [FromQuery] ExpertApplicationStatus? status = null, [FromQuery] Guid? acceptedBy = null)
    {
        try
        {
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);
            var isAdmin = User.IsInRole("admin") || User.IsInRole("superadmin");
            var currentUserId = JwtHelper.GetUserIdFromUser(User);

            var query = new GetAllExpertApplicationsQuery(take, orderParams.OrderType, orderParams.FieldName,
                cursorParams.CursorType, cursorParams.CursorId, currentUserId, isAdmin, createdBy, status, acceptedBy);

            var response = await mediator.Send(query);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }

    [HttpGet("{id:guid}", Name = nameof(GetExpertApplicationById))]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> GetExpertApplicationById([FromRoute] Guid id)
    {
        try
        {
            var isAdmin = User.IsInRole("admin") || User.IsInRole("superadmin");
            var currentUserId = JwtHelper.GetUserIdFromUser(User);

            var query = new GetExpertApplicationByIdQuery(id, currentUserId, isAdmin);
            var response = await mediator.Send(query);

            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }


    [HttpPost("{id:guid}/send")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> SendExpertApplicationController([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            var command = new SendExpertApplicationCommand(id, userId);

            await mediator.Send(command);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/cancelSending")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> CancelSendingExpertApplication([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            var command = new CancelSendingExpertApplicationCommand(id, userId);
            await mediator.Send(command);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/accept")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AcceptForReviewExpertApplicationController([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new AcceptForReviewExpertApplicationCommand(id, userId);
            await mediator.Send(command);
         
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/cancelAcceptance")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CancelAcceptanceExpertApplicationController([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new CancelAcceptanceExpertApplicationCommand(id, userId);
            await mediator.Send(command);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/review")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> ReviewExpertApplicationController([FromRoute] Guid id,
        [FromBody] CompleteReviewExpertApplicationRequestDto request)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new ReviewExpertApplicationCommand(id, userId, request.IsApproved, request.Message);
            await mediator.Send(command);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{id:guid}/cancelReview")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CancelReviewExpertApplicationController([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            var command = new CancelReviewExpertApplicationCommand(id, userId);
            await mediator.Send(command);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }


    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> PatchExpertApplicationController([FromRoute] Guid id,
        [FromBody] JsonPatchDocument<PatchExpertApplicationRequestDto> request)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new PatchExpertApplicationCommand(id, userId, request);
            var response = await mediator.Send(command);

            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> DeleteExpertApplicationController([FromRoute] Guid id)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);

            var command = new DeleteExpertApplicationCommand(userId, id);
            await mediator.Send(command);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpGet("{ig:guid}/history")]
    [Authorize(Policy = "logged_in")]
    public async Task<IActionResult> GetApplicationHistoryController([FromRoute] Guid id,
        [FromQuery] string? cursor = null,
        [FromQuery] int take = 10,
        [FromQuery] string? order = null)
    {
        try
        {
            var userId = JwtHelper.GetUserIdFromUser(User);
            var orderParams = QueryParamsParser.ParseOrderParams(order);
            var cursorParams = QueryParamsParser.ParseCursorParams(cursor);

            var query = new GetApplicationHistoryQuery(take, orderParams.OrderType, orderParams.FieldName,
                cursorParams.CursorType, cursorParams.CursorId, userId, id);
            
            var response = await mediator.Send(query);
            
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
    }
}