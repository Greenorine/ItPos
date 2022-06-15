using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Applications;

public record GetApplicationById(Guid Id) : IRequest<Application>;

public class GetApplicationByIdHandler : IRequestHandler<GetApplicationById, Application>
{
    private readonly ItPosDbContext context;

    public GetApplicationByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Application> Handle(GetApplicationById request,
        CancellationToken cancellationToken)
    {
        var application =
            await context.Applications.AsNoTracking().Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return application ?? throw new EntityNotFoundException(request.Id.ToString());
    }
}