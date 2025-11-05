using MediatR;
using YTH_backend.DTOs.User;

namespace YTH_backend.Features.Users.Queries;
//TODO
public record GetPersonalDataQuery() :IRequest<GetPersonalDataResponseDto>;