using MediatR;

namespace YTH_backend.Features.AdminAppointments.Commands;

public record AddAdminCommand(Guid AppointorId, Guid AppointeeId) : IRequest;