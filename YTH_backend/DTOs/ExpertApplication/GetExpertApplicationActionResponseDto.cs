using System.Text.Json.Serialization;
using YTH_backend.Enums;

namespace YTH_backend.DTOs.ExpertApplication;

[JsonConverter(typeof(JsonStringEnumConverter))]
public record GetExpertApplicationActionResponseDto(Guid Id, Guid ExpertApplicationId, DateTime CreatedAt, ExpertApplicationActionType ExpertApplicationActionType, string? Other);