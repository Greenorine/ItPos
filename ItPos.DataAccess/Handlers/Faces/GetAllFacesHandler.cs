using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Faces;

public record GetAllFaces : IRequest<List<Face>>;

public class GetAllFacesHandler : IRequestHandler<GetAllFaces, List<Face>>
{
    private readonly ItPosDbContext context;

    public GetAllFacesHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Face>> Handle(GetAllFaces request,
        CancellationToken cancellationToken)
    {
        return await context.Faces.AsNoTracking().Where(c => !c.IsDeleted)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}