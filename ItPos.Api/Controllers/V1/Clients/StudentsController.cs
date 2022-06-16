using ItPos.DataAccess.Handlers.Students;
using ItPos.Domain.DTO;
using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItPos.Api.Controllers.V1.Clients;

[Produces("application/json")]
[Route("api/v1/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IMediator mediator;

    public StudentsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить студента по Id.
    /// </summary>
    /// <param name="request">Запрос с Id</param>
    /// <response code="200">Информация о студенте</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(StudentInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get")]
    public async Task<IActionResult> Get(RequestWithId request, CancellationToken token)
    {
        var result = await mediator.Send(new GetStudentById(request.Id), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет получить всех студентов.
    /// </summary>
    /// <response code="200">Список студентов</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(List<StudentInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("get_all")]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var result = await mediator.Send(new GetAllStudents(), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет сохранить/обновить запись о студенте.
    /// </summary>
    /// <param name="request">
    /// Запрос с информацией о студенте<br/>
    /// <i>Указывайте <b>Id</b>, если хотите изменить существующую запись<br/>
    /// В противном случае - не передавайте его</i>
    /// </param>
    /// <response code="200">Информация о созданном/измененном студенте</response>
    /// <response code="400">Ошибка</response>
    [ProducesResponseType(typeof(StudentInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]

    #endregion
    [HttpPost("save")]
    public async Task<IActionResult> Save(StudentInfoRequest request, CancellationToken token)
    {
        var result = await mediator.Send(new SaveStudent(request), token);
        return Ok(result);
    }
    
    #region SwaggerDoc

    /// <summary>
    /// Позволяет удалить студента по Id.
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
        await mediator.Publish(new DeleteStudentById(request.Id), token);
        return NoContent();
    }
}