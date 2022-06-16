using ItPos.Domain.Exceptions;
using ItPos.Domain.Models.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Users;

public record GetUserByEmail(string Email) : IRequest<PosUser>;

public class GetUserByEmailHandler : IRequestHandler<GetUserByEmail, PosUser>
{
    private readonly ItPosDbContext context;

    public GetUserByEmailHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<PosUser> Handle(GetUserByEmail request,
        CancellationToken cancellationToken)
    {
        var client =
            await context.Users.AsNoTracking().Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(x => x.Login == request.Email, cancellationToken: cancellationToken);
        return client ?? throw new EntityNotFoundException(nameof(request.Email), request.Email);
    }
}