using ItPos.DataAccess.Handlers.Offers;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Offer;
using ItPos.Domain.Models;
using ItPos.Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Offers;

[Produces("application/json")]
[Route("api/v1/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly IMediator mediator;

    public OffersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить предложение по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="200">Информация о предложении</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Offer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetOfferById(request.Id), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить все предложения.
    /// </summary>
    /// <response code="200">Список предложений</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(List<Offer>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllOffers(), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет сохранить/обновить запись о предложении.
    /// </summary>
    /// <param name="request">
    /// Запрос с информацией о предложении<br/>
    /// <i>Указывайте <b>Id</b>, если хотите изменить существующую запись<br/>
    /// В противном случае - не передавайте его</i>
    /// </param>
    /// <response code="200">Информация о созданном/измененном предложении</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(Offer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("save")]
    public async Task<IActionResult> Save(OfferRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveOffer(request), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет удалить предложение по Id.
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
        await mediator.Publish(new DeleteOfferById(request.Id), token);
        return NoContent();
    }
}