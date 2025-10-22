using MediatR;

namespace YTH_backend.Features.AdminAppointments.Commands;

public record RemoveAdminCommand(Guid RevokedId) : IRequest;