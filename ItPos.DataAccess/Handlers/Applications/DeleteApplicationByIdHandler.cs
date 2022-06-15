using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Applications;

public record DeleteApplicationById(Guid Id) : INotification;

public class DeleteApplicationByIdHandler : INotificationHandler<DeleteApplicationById>
{
    private readonly ItPosDbContext context;

    public DeleteApplicationByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteApplicationById request,
        CancellationToken cancellationToken)
    {
        var application = await context.Applications.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
            cancellationToken);
        if (application is null) return;
        context.Remove(application);

        await context.SaveChangesAsync(cancellationToken);
    }
}