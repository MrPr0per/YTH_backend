using YTH_backend.Enums;

namespace YTH_backend.DTOs.ExpertApplication;

public record CreateExpertApplicationResponseDto(Guid Id, Guid UserId, string Message, ExpertApplicationStatus Status);