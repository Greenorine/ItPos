using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Faces;

public record GetFaceById(Guid Id) : IRequest<Face>;

public class GetFaceByIdHandler : IRequestHandler<GetFaceById, Face>
{
    private readonly ItPosDbContext context;

    public GetFaceByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Face> Handle(GetFaceById request,
        CancellationToken cancellationToken)
    {
        var face =
            await context.Faces.AsNoTracking().Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return face ?? throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());
    }
}