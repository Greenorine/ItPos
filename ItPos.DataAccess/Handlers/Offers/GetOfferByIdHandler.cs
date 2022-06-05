using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Offers;

public record GetOfferById(Guid Id) : IRequest<Result<Offer>>;

public class GetOfferByIdHandler : IRequestHandler<GetOfferById, Result<Offer>>
{
    private readonly ItPosDbContext context;

    public GetOfferByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Result<Offer>> Handle(GetOfferById request,
        CancellationToken cancellationToken)
    {
        var client =
            await context.Offers.AsNoTracking().Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return client ?? new Result<Offer>(new EntityNotFoundException(request.Id.ToString()));
    }
}