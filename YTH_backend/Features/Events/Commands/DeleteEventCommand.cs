using System.Windows.Input;
using MediatR;

namespace YTH_backend.Features.Events.Commands;

public record DeleteEventCommand(Guid EventId) : IRequest;