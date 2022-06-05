using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Offers;

public record DeleteOfferById(Guid Id) : INotification;

public class DeleteOfferByIdHandler : INotificationHandler<DeleteOfferById>
{
    private readonly ItPosDbContext context;

    public DeleteOfferByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteOfferById request,
        CancellationToken cancellationToken)
    {
        var offer = await context.Offers.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
            cancellationToken);
        if (offer is null) return;
        context.Remove(offer);

        await context.SaveChangesAsync(cancellationToken);
    }
}