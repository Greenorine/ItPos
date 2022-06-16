using ItPos.DataAccess.Handlers.Applications;
using ItPos.DataAccess.Handlers.Users;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Application;
using ItPos.Domain.DTO.V1.Auth;
using ItPos.Domain.Models.Response;
using ItPos.Infrastructure.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Auth;

[Produces("application/json")]
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

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить юзера по Id.
    /// </summary>
    /// <param name="request">Запрос с логином и паролем</param>
    /// <response code="200">Информация о токене</response>
    /// <response code="400">Введены некорректный пароль или имя пользователя</response>
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("login")]
    public async Task<IActionResult> Get(LoginRequest request, CancellationToken token)
    {
        var user = await mediator.Send(new GetUserByEmail(request.Login), token);
        if (user.Password != request.Password) return BadRequest("Введены некорректный пароль или имя пользователя");
        var expiresAt = DateTime.Now.AddHours(1);
        var jwtToken = tokenManager.GenerateToken(user, expiresAt);
        return Ok(new LoginResponse(jwtToken, expiresAt));
    }
}