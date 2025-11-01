using MediatR;

namespace YTH_backend.Features.AdminAppointments.Commands;

public record AddAdminCommand(Guid UserId) : IRequest;