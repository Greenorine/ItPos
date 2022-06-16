using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Faces;

public record DeleteFaceById(Guid Id) : INotification;

public class DeleteFaceByIdHandler : INotificationHandler<DeleteFaceById>
{
    private readonly ItPosDbContext context;

    public DeleteFaceByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteFaceById request,
        CancellationToken cancellationToken)
    {
        var form = await context.Faces.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
            cancellationToken);
        if (form is null) return;
        context.Remove(form);

        await context.SaveChangesAsync(cancellationToken);
    }
}