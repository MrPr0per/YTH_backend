using MediatR;
using YTH_backend.DTOs.ExpertApplication;
using YTH_backend.Enums;
using YTH_backend.Models.Infrastructure;

namespace YTH_backend.Features.ExpertApplication.Queries;

public record GetAllExpertApplicationsQuery(
    int Take,
    OrderType OrderType,
    string OrderFieldName,
    CursorType CursorType,
    Guid? CursorId,
    Guid CurrentUserId,
    bool IsAdmin,
    Guid? CreatedBy,
    ExpertApplicationStatus? Status,
    Guid? AcceptedBy) : IRequest<PagedResult<GetExpertApplicationResponseDto>>;