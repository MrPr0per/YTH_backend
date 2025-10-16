using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YTH_backend.Controllers;

[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;
}