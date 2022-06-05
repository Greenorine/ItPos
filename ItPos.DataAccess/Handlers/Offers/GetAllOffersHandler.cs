using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Offers;

public record GetAllOffers : IRequest<List<Offer>>;

public class GetAllOffersHandler : IRequestHandler<GetAllOffers, List<Offer>>
{
    private readonly ItPosDbContext context;

    public GetAllOffersHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Offer>> Handle(GetAllOffers request,
        CancellationToken cancellationToken)
    {
        return await context.Offers.AsNoTracking().Where(c => !c.IsDeleted)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}