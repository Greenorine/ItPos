using ItPos.DataAccess.Handlers.Faces;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Face;
using ItPos.Domain.Models;
using ItPos.Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Faces;

[Produces("application/json")]
[Route("api/v1/[controller]")]
[ApiController]
public class FacesController : ControllerBase
{
    private readonly IMediator mediator;

    public FacesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить лицо POS по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="200">Информация о лице POS</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Face), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetFaceById(request.Id), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить все лица POS.
    /// </summary>
    /// <response code="200">Список лиц POS</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(List<Face>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllFaces(), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет сохранить/обновить запись о лице POS.
    /// </summary>
    /// <param name="request">
    /// Запрос с информацией о лице POS<br/>
    /// <i>Указывайте <b>Id</b>, если хотите изменить существующую запись<br/>
    /// В противном случае - не передавайте его</i>
    /// </param>
    /// <response code="200">Информация о созданном/измененном лице POS</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Face), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("save")]
    public async Task<IActionResult> Save(FaceRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveFace(request), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет удалить лицо POS по Id.
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
        await mediator.Publish(new DeleteFaceById(request.Id), token);
        return NoContent();
    }
}