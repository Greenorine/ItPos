using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Forms;

public record DeleteFormById(Guid Id) : INotification;

public class DeleteFormByIdHandler : INotificationHandler<DeleteFormById>
{
    private readonly ItPosDbContext context;

    public DeleteFormByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteFormById request,
        CancellationToken cancellationToken)
    {
        var form = await context.Forms.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
            cancellationToken);
        if (form is null) return;
        context.Remove(form);

        await context.SaveChangesAsync(cancellationToken);
    }
}