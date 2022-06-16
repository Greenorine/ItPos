using ItPos.DataAccess.Handlers.Applications;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Application;
using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Models;
using ItPos.Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Applications;

[Produces("application/json")]
[Route("api/v1/[controller]")]
[ApiController]
public class ApplicationsController : ControllerBase
{
    private readonly IMediator mediator;

    public ApplicationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить заявление по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="200">Информация о заявлении</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Application), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetApplicationById(request.Id), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить все заявления.
    /// </summary>
    /// <response code="200">Список заявлений</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(List<Application>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllApplications(), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет опубликовать заявление.
    /// </summary>
    /// <param name="request">
    /// Запрос с информацией о заявлении, содержит заполненные поля формы и Id самой формы
    /// </param>
    /// <response code="200">Информация об опубликованном заявлении</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Application), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("publish")]
    public async Task<IActionResult> Publish(ApplicationRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new PublishApplication(request), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет удалить заявление по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="204">Запись удалена</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(RequestWithId request, CancellationToken token)
    {
        await mediator.Publish(new DeleteApplicationById(request.Id), token);
        return NoContent();
    }
}