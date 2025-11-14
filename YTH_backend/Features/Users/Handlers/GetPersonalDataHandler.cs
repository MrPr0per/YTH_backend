using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Queries;

namespace YTH_backend.Features.Users.Handlers;

public class GetPersonalDataHandler(AppDbContext dbContext) : IRequestHandler<GetPersonalDataQuery, GetPersonalDataResponseDto>
{
    public Task<GetPersonalDataResponseDto> Handle(GetPersonalDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}