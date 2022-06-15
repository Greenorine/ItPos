using ItPos.DataAccess.User;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Users;

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
        return Ok(result);
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(UserRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveUser(request), token);
        return Ok(result);
    }
}