using ItPos.Domain.Exceptions;
using ItPos.Domain.Models.User;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.User;

public record GetUserById(Guid Id) : IRequest<Result<PosUser>>;

public class GetUserByIdHandler : IRequestHandler<GetUserById, Result<PosUser>>
{
    private readonly ItPosDbContext context;

    public GetUserByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Result<PosUser>> Handle(GetUserById request,
        CancellationToken cancellationToken)
    {
        var client =
            await context.Users.AsNoTracking().Where(u => !u.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return client ?? new Result<PosUser>(new EntityNotFoundException(request.Id.ToString()));
    }
}