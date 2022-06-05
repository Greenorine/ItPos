using ItPos.DataAccess.User;
using ItPos.Domain.DTO.StudentInfo;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Extensions;
using ItPos.Domain.Models;
using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Student;

public record SaveStudent(StudentInfoRequest StudentData) : IRequest<Result<StudentInfo>>;

public class SaveStudentHandler : IRequestHandler<SaveStudent, Result<StudentInfo>>
{
    private readonly ItPosDbContext context;
    private static readonly TypeAdapterConfig Config = new();
    private readonly IMediator mediator;

    public SaveStudentHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
        Config.NewConfig<StudentInfoRequest, StudentInfo>().GenerateMapper(MapType.Projection).Ignore(x => x.Id!)
            .CompileProjection();
    }

    public async Task<Result<StudentInfo>> Handle(SaveStudent request, CancellationToken token)
    {
        Result<StudentInfo> studentInfo;
        if (!request.StudentData.Id.HasValue)
            studentInfo = await CreateStudentInfo(request.StudentData, token);
        else
            studentInfo = await UpdateStudentInfo(request.StudentData, token);
        return studentInfo;
    }

    private async Task<Result<StudentInfo>> CreateStudentInfo(StudentInfoRequest request, CancellationToken token)
    {
        var studentInfo = request.Adapt<StudentInfo>();
        if (!await TryCreateUser(request, token, studentInfo))
            return new Result<StudentInfo>(new Exception("Не удалось создать пользователя."));
        context.Students.Add(studentInfo);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            return new Result<StudentInfo>(new EntityExistsException());
        }

        return studentInfo;
    }

    private async Task<bool> TryCreateUser(StudentInfoRequest request, CancellationToken token, StudentInfo studentInfo)
    {
        var id = (await mediator.Send(new SaveUser(request.User), token)).ToObjectOrDefault()?.Id;
        if (id is null) return false;

        await context.SaveChangesAsync(token);
        studentInfo.UserId = id.Value;
        return true;
    }

    private async Task<Result<StudentInfo>> UpdateStudentInfo(StudentInfoRequest request, CancellationToken token)
    {
        var studentInfo =
            await context.Students.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken: token);

        if (studentInfo is null)
            return new Result<StudentInfo>(new EntityNotFoundException(request.Id.ToString()));

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == studentInfo.UserId, cancellationToken: token);
        if (user is null)
        {
            if (!await TryCreateUser(request, token, studentInfo))
                return new Result<StudentInfo>(new Exception("Не удалось создать пользователя."));
        }
        else if (user.Login != request.User.Login || user.Password != request.User.Password)
            await mediator.Send(new SaveUser(request.User), token);

        request.Adapt(studentInfo, Config);
        context.Update(studentInfo);
        await context.SaveChangesAsync(token);
        return studentInfo;
    }
}