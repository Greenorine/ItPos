using ItPos.DataAccess.Handlers.Users;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.User;
using ItPos.Domain.Models.Response;
using ItPos.Domain.Models.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Users;

[Produces("application/json")]
[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить юзера по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="200">Информация о юзере</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(PosUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetUserById(request.Id), token);
        return Ok(result);
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет сохранить/обновить запись о юзере.
    /// </summary>
    /// <param name="request">
    /// Запрос с информацией о юзере<br/>
    /// <i>Указывайте <b>Id</b>, если хотите изменить существующую запись<br/>
    /// В противном случае - не передавайте его</i>
    /// </param>
    /// <response code="200">Информация о созданном/измененном юзере</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(PosUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("save")]
    public async Task<IActionResult> Save(UserRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveUser(request), token);
        return Ok(result);
    }
}