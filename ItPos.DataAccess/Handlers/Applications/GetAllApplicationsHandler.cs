using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Applications;

public record GetAllApplications : IRequest<List<Application>>;

public class GetAllApplicationsHandler : IRequestHandler<GetAllApplications, List<Application>>
{
    private readonly ItPosDbContext context;

    public GetAllApplicationsHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Application>> Handle(GetAllApplications request,
        CancellationToken cancellationToken)
    {
        return await context.Applications.AsNoTracking().Where(c => !c.IsDeleted)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}