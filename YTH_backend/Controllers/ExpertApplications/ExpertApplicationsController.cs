using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Features.ExpertApplication.Commands;

namespace YTH_backend.Controllers.ExpertApplications;

[ApiController]
[Route("api/v0/expertApplications")]
public class ExpertApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> CreateExpertApplicationController([FromBody] CreateExpertApplicationRequestDto request)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        
        var command = new CreateExpertApplicationCommand(userId, request.Message);
        throw new NotImplementedException();
    }

    [HttpPost("{id:guid}/send")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> SendExpertApplicationController(Guid id)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        
        var command = new SendExpertApplicationCommand(id, userId);
        throw new NotImplementedException();
    }

    [HttpPost("{id:guid}/recall")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> RecallExpertApplicationController(Guid id)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        var command = new RecallExpertApplicationCommand(id, userId);
        throw new NotImplementedException();
    }
    
    [HttpPost("{id:guid}/acceptedForReview")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> AcceptForReviewExpertApplicationController(Guid id)
    {
        var command = new AcceptForReviewExpertApplicationCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPost("{id:guid}/cancelReview")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> CancelForReviewExpertApplicationController(Guid id)
    { 
        var command = new CancelForReviewExpertApplicationCommand(id);
        throw new NotImplementedException();
    }
    
    [HttpPost("{id:guid}/completeReview")]
    [Authorize(Roles = "admin,superadmin")]
    public async Task<IActionResult> CompleteReviewExpertApplicationController(Guid id, [FromBody] CompleteReviewExpertApplicationRequestDto request)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);

        var command = new CompleteReviewExpertApplicationCommand(id, request.ResolutionId, userId);
        throw new NotImplementedException();
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> PatchExpertApplicationController(Guid id,
        [FromBody] JsonPatchDocument<PatchExpertApplicationRequestDto> request)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        
        var command = new PatchExpertApplicationCommand(id, userId, request);
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "student,superadmin")]
    public async Task<IActionResult> DeleteExpertApplicationController(Guid id)
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? throw new UnauthorizedAccessException("User ID not found in token");

        var userId = Guid.Parse(userIdClaim);
        var command = new DeleteExpertApplicationCommand(userId, id);
        throw new NotImplementedException();
    }
}