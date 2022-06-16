using ItPos.Domain.DTO.V1.Offer;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Offers;

public record SaveOffer(OfferRequest OfferData) : IRequest<Offer>;

public class SaveOfferHandler : IRequestHandler<SaveOffer, Offer>
{
    private readonly ItPosDbContext context;
    private readonly IMediator mediator;

    public SaveOfferHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
    }

    public async Task<Offer> Handle(SaveOffer request, CancellationToken token)
    {
        Offer Offer;
        if (!request.OfferData.Id.HasValue)
            Offer = await CreateOffer(request.OfferData, token);
        else
            Offer = await UpdateOffer(request.OfferData, token);
        return Offer;
    }

    private async Task<Offer> CreateOffer(OfferRequest request, CancellationToken token)
    {
        var offer = request.Adapt<Offer>();
        context.Offers.Add(offer);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            throw new EntityExistsException();
        }

        return offer;
    }

    private async Task<Offer> UpdateOffer(OfferRequest request, CancellationToken token)
    {
        var offer =
            await context.Offers.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken: token);

        if (offer is null)
            throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());

        request.Adapt(offer);
        context.Update(offer);
        await context.SaveChangesAsync(token);
        return offer;
    }
}