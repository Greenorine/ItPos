using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Offers;

public record GetOfferById(Guid Id) : IRequest<Offer>;

public class GetOfferByIdHandler : IRequestHandler<GetOfferById, Offer>
{
    private readonly ItPosDbContext context;

    public GetOfferByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Offer> Handle(GetOfferById request,
        CancellationToken cancellationToken)
    {
        var offer =
            await context.Offers.AsNoTracking().Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return offer ?? throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());
    }
}