using ItPos.DataAccess.Handlers.Forms;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Form;
using ItPos.Domain.Models;
using ItPos.Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Forms;

[Produces("application/json")]
[Route("api/v1/[controller]")]
[ApiController]
public class FormsController : ControllerBase
{
    private readonly IMediator mediator;

    public FormsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить форму по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="200">Информация о форме</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Form), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetFormById(request.Id), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить все формы.
    /// </summary>
    /// <response code="200">Список форм</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(List<Form>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllForms(), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет сохранить/обновить запись о форме.
    /// </summary>
    /// <param name="request">
    /// Запрос с информацией о форме<br/>
    /// <i>Указывайте <b>Id</b>, если хотите изменить существующую запись<br/>
    /// В противном случае - не передавайте его</i>
    /// </param>
    /// <response code="200">Информация о созданной/измененной форме</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Form), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("save")]
    public async Task<IActionResult> Save(FormRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveForm(request), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет удалить форму по Id.
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
        await mediator.Publish(new DeleteFormById(request.Id), token);
        return NoContent();
    }
}