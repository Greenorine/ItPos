using ItPos.Domain.Exceptions;
using ItPos.Domain.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Users;

public record GetUserById(Guid Id) : IRequest<PosUser>;

public class GetUserByIdHandler : IRequestHandler<GetUserById, PosUser>
{
    private readonly ItPosDbContext context;

    public GetUserByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<PosUser> Handle(GetUserById request,
        CancellationToken cancellationToken)
    {
        var client =
            await context.Users.AsNoTracking().Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return client ?? throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());
    }
}