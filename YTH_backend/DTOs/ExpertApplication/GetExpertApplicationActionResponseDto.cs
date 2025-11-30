using YTH_backend.Enums;

namespace YTH_backend.DTOs.ExpertApplication;

public record GetExpertApplicationActionResponseDto(Guid Id, Guid ExpertApplicationId, DateTime CreatedAt, ExpertApplicationActionType ExpertApplicationActionType, string? Other);