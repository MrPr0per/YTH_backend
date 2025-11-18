using MediatR;
using YTH_backend.DTOs.Event;
using YTH_backend.Enums;

namespace YTH_backend.Features.Events.Commands;

public record AddEventCommand(string Name, string Description, EventTypes Type, DateTime Date, string? Address) : IRequest<AddEventResponseDto>;