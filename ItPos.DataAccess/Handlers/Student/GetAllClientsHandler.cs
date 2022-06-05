using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Student;

public record GetAllStudents : IRequest<List<StudentInfo>>;

public class GetAllClientsHandler : IRequestHandler<GetAllStudents, List<StudentInfo>>
{
    private readonly ItPosDbContext context;

    public GetAllClientsHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<List<StudentInfo>> Handle(GetAllStudents request,
        CancellationToken cancellationToken)
    {
        return await context.Students.AsNoTracking().Where(c => !c.IsDeleted).ToListAsync(cancellationToken: cancellationToken);
    }
}