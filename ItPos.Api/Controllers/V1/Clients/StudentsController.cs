using ItPos.DataAccess.Handlers.Students;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.StudentInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Clients;

[Route("api/v1/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IMediator mediator;

    public StudentsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetStudentById(request.Id), token);
        return Ok(result);
    }
    
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllStudents(), token);
        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save(StudentInfoRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveStudent(request), token);
        return Ok(result);
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(RequestWithId request, CancellationToken token)
    {
        await mediator.Publish(new DeleteStudentById(request.Id), token);
        return NoContent();
    }
}