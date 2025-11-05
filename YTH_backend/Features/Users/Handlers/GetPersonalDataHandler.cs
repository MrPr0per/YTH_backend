using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.User;
using YTH_backend.Features.Users.Queries;

namespace YTH_backend.Features.Users.Handlers;

public class GetPersonalDataHandler(AppDbContext context) : IRequestHandler<GetPersonalDataQuery, GetPersonalDataResponseDto>
{
    private readonly AppDbContext dbContext = context;
    
    public Task<GetPersonalDataResponseDto> Handle(GetPersonalDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}