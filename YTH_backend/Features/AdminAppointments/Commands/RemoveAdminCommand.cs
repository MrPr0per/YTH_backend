using MediatR;

namespace YTH_backend.Features.AdminAppointments.Commands;

public record RemoveAdminCommand(Guid RevokerId, Guid AppointeeId, string Reason) : IRequest;