using ItPos.DataAccess.Handlers.Forms;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Form;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Forms;

[Route("api/v1/[controller]")]
[ApiController]
public class FormsController : ControllerBase
{
    private readonly IMediator mediator;

    public FormsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetFormById(request.Id), token);
        return Ok(result);
    }
    
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllForms(), token);
        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save(FormRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveForm(request), token);
        return Ok(result);
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(RequestWithId request, CancellationToken token)
    {
        await mediator.Publish(new DeleteFormById(request.Id), token);
        return NoContent();
    }
}