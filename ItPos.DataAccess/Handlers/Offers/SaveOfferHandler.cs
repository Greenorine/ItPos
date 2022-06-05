using ItPos.DataAccess.User;
using ItPos.Domain.DTO.Offer;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Extensions;
using ItPos.Domain.Models;
using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Offers;

public record SaveOffer(OfferRequest OfferData) : IRequest<Result<Offer>>;

public class SaveOfferHandler : IRequestHandler<SaveOffer, Result<Offer>>
{
    private readonly ItPosDbContext context;
    private static readonly TypeAdapterConfig Config = new();
    private readonly IMediator mediator;

    public SaveOfferHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
        Config.NewConfig<OfferRequest, Offer>().GenerateMapper(MapType.Projection).Ignore(x => x.Id!)
            .CompileProjection();
    }

    public async Task<Result<Offer>> Handle(SaveOffer request, CancellationToken token)
    {
        Result<Offer> Offer;
        if (!request.OfferData.Id.HasValue)
            Offer = await CreateOffer(request.OfferData, token);
        else
            Offer = await UpdateOffer(request.OfferData, token);
        return Offer;
    }

    private async Task<Result<Offer>> CreateOffer(OfferRequest request, CancellationToken token)
    {
        var offer = request.Adapt<Offer>();
        context.Offers.Add(offer);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            return new Result<Offer>(new EntityExistsException());
        }

        return offer;
    }

    private async Task<Result<Offer>> UpdateOffer(OfferRequest request, CancellationToken token)
    {
        var offer =
            await context.Offers.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken: token);

        if (offer is null)
            return new Result<Offer>(new EntityNotFoundException(request.Id.ToString()));

        request.Adapt(offer, Config);
        context.Update(offer);
        await context.SaveChangesAsync(token);
        return offer;
    }
}