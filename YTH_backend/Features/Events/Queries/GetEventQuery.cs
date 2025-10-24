using MediatR;
using YTH_backend.DTOs.Event;

namespace YTH_backend.Features.Events.Queries;

public record GetEventQuery(Guid EventId) : IRequest<GetEventResponseDto>;