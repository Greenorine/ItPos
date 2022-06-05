using ItPos.Api.Extensions;
using ItPos.DataAccess.User;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.Users;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetUserById(request.Id), token);
        return result.ToResponse();
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(UserRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveUser(request), token);
        return result.ToResponse();
    }
}