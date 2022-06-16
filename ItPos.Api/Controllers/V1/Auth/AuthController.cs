using ItPos.DataAccess.Handlers.Applications;
using ItPos.DataAccess.Handlers.Users;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Application;
using ItPos.Domain.DTO.V1.Auth;
using ItPos.Infrastructure.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Auth;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly ITokenManager tokenManager;

    public AuthController(IMediator mediator, ITokenManager tokenManager)
    {
        this.mediator = mediator;
        this.tokenManager = tokenManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Get(LoginRequest request, CancellationToken token)
    {
        var user = await mediator.Send(new GetUserByEmail(request.Login), token);
        if (user.Password != request.Password) return BadRequest("Введены некорректный пароль или имя пользователя");
        var expiresAt = DateTime.Now.AddHours(1);
        var jwtToken = tokenManager.GenerateToken(user, expiresAt);
        return Ok(new {jwtToken, expiresAt});
    }
}