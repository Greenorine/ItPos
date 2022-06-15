using ItPos.DataAccess.User;
using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.DTO.V1.User;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Student;

public record SaveStudent(StudentInfoRequest StudentData) : IRequest<StudentInfoResponse>;

public class SaveStudentHandler : IRequestHandler<SaveStudent, StudentInfoResponse>
{
    private readonly ItPosDbContext context;
    private readonly IMediator mediator;

    public SaveStudentHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
    }

    public async Task<StudentInfoResponse> Handle(SaveStudent request, CancellationToken token)
    {
        StudentInfoResponse studentInfo;
        if (!request.StudentData.Id.HasValue)
            studentInfo = await CreateStudentInfo(request.StudentData, token);
        else
            studentInfo = await UpdateStudentInfo(request.StudentData, token);
        return studentInfo;
    }

    private async Task<StudentInfoResponse> CreateStudentInfo(StudentInfoRequest request, CancellationToken token)
    {
        var studentInfo = request.Adapt<StudentInfo>();
        try
        {
            studentInfo.User = await mediator.Send(new SaveUser(CreateUserFromRequest(request)), token);
        }
        catch (EntityExistsException ex)
        {
            throw new LoginIsBusyException(request.Login, ex);
        }

        context.Students.Add(studentInfo);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            throw new EntityExistsException();
        }

        return studentInfo.Adapt<StudentInfoResponse>();
    }

    private async Task<StudentInfoResponse> UpdateStudentInfo(StudentInfoRequest request, CancellationToken token)
    {
        var studentInfo =
            await context.Students.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken: token);

        if (studentInfo is null)
            throw new EntityNotFoundException(request.Id.ToString());

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == studentInfo.User.Id, cancellationToken: token);
        try
        {
            if (user is null)
                studentInfo.User = await mediator.Send(new SaveUser(CreateUserFromRequest(request)), token);
            else if (user.Login != request.Login || user.Password != request.Password)
                studentInfo.User = await mediator.Send(new SaveUser(CreateUserFromRequest(request)), token);
        }
        catch (EntityExistsException ex)
        {
            throw new LoginIsBusyException(request.Login, ex);
        }

        request.Adapt(studentInfo);
        context.Update(studentInfo);
        await context.SaveChangesAsync(token);
        return studentInfo.Adapt<StudentInfoResponse>();
    }

    private static UserRequest CreateUserFromRequest(StudentInfoRequest request)
    {
        return new UserRequest
        {
            Group = request.Institute.ToString(),
            Login = request.Login,
            Password = request.Password,
            Roles = "Student"
        };
    }
}