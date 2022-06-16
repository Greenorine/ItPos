using ItPos.DataAccess.Handlers.Faces;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Face;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Faces;

[Route("api/v1/[controller]")]
[ApiController]
public class FacesController : ControllerBase
{
    private readonly IMediator mediator;

    public FacesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetFaceById(request.Id), token);
        return Ok(result);
    }
    
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllFaces(), token);
        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save(FaceRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveFace(request), token);
        return Ok(result);
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(RequestWithId request, CancellationToken token)
    {
        await mediator.Publish(new DeleteFaceById(request.Id), token);
        return NoContent();
    }
}