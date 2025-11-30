using YTH_backend.Enums;

namespace YTH_backend.DTOs.ExpertApplication;

public record GetExpertApplicationResponseDto(
    Guid Id,
    Guid UserId,
    string Message,
    ExpertApplicationStatus ExpertApplicationStatus,
    Guid? AcceptedBy,
    bool? IsApproved,
    string? ResolutionMessage);