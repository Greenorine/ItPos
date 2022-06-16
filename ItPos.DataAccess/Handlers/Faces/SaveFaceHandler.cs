using ItPos.Domain.DTO.V1.Face;
using ItPos.Domain.DTO.V1.Form;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Faces;

public record SaveFace(FaceRequest FaceData) : IRequest<Face>;

public class SaveFaceHandler : IRequestHandler<SaveFace, Face>
{
    private readonly ItPosDbContext context;
    private readonly IMediator mediator;

    public SaveFaceHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
    }

    public async Task<Face> Handle(SaveFace request, CancellationToken token)
    {
        Face face;
        if (!request.FaceData.Id.HasValue)
            face = await CreateFace(request.FaceData, token);
        else
            face = await UpdateFace(request.FaceData, token);
        return face;
    }

    private async Task<Face> CreateFace(FaceRequest request, CancellationToken token)
    {
        var face = request.Adapt<Face>();
        context.Faces.Add(face);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            throw new EntityExistsException();
        }

        return face;
    }

    private async Task<Face> UpdateFace(FaceRequest request, CancellationToken token)
    {
        var face =
            await context.Faces.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken: token);

        if (face is null)
            throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());

        request.Adapt(face);
        context.Update(face);
        await context.SaveChangesAsync(token);
        return face;
    }
}