using ItPos.DataAccess.Handlers.Offers;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Offer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Offers;

[Route("api/v1/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly IMediator mediator;

    public OffersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetOfferById(request.Id), token);
        return Ok(result);
    }
    
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllOffers(), token);
        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save(OfferRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveOffer(request), token);
        return Ok(result);
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(RequestWithId request, CancellationToken token)
    {
        await mediator.Publish(new DeleteOfferById(request.Id), token);
        return NoContent();
    }
}