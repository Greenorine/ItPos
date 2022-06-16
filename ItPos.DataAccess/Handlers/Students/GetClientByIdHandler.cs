using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Exceptions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Students;

public record GetStudentById(Guid Id) : IRequest<StudentInfoResponse>;

public class GetClientByIdHandler : IRequestHandler<GetStudentById, StudentInfoResponse>
{
    private readonly ItPosDbContext context;

    public GetClientByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<StudentInfoResponse> Handle(GetStudentById request,
        CancellationToken cancellationToken)
    {
        var studentInfo = await context.Students.AsNoTracking().Include(x => x.User).Where(c => !c.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (studentInfo is null)
            throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());
        
        return studentInfo.Adapt<StudentInfoResponse>();
    }
}