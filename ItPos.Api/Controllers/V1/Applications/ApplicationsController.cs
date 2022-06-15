using ItPos.DataAccess.Handlers.Applications;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Applications;

[Route("api/v1/[controller]")]
[ApiController]
public class ApplicationsController : ControllerBase
{
    private readonly IMediator mediator;

    public ApplicationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetApplicationById(request.Id), token);
        return Ok(result);
    }
    
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllApplications(), token);
        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save(ApplicationRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new PublishApplication(request), token);
        return Ok(result);
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(RequestWithId request, CancellationToken token)
    {
        await mediator.Publish(new DeleteApplicationById(request.Id), token);
        return NoContent();
    }
}