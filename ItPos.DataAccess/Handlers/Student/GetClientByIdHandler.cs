using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Student;

public record GetStudentById(Guid Id) : IRequest<Result<StudentInfo>>;

public class GetClientByIdHandler : IRequestHandler<GetStudentById, Result<StudentInfo>>
{
    private readonly ItPosDbContext context;

    public GetClientByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Result<StudentInfo>> Handle(GetStudentById request,
        CancellationToken cancellationToken)
    {
        var client =
            await context.Students.AsNoTracking().Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return client ?? new Result<StudentInfo>(new EntityNotFoundException(request.Id.ToString()));
    }
}