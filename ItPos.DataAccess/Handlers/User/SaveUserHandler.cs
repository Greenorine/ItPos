using ItPos.Domain.DTO.User;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Models.User;
using LanguageExt.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.User;

public record SaveUser(UserRequest UserData) : IRequest<Result<PosUser>>;

public class SaveUserHandler : IRequestHandler<SaveUser, Result<PosUser>>
{
    private readonly ItPosDbContext context;
    private static readonly TypeAdapterConfig Config = new();

    public SaveUserHandler(ItPosDbContext context)
    {
        this.context = context;
        Config.NewConfig<UserRequest, PosUser>().GenerateMapper(MapType.Projection).Ignore(x => x.Id!).CompileProjection();
    }

    public async Task<Result<PosUser>> Handle(SaveUser request, CancellationToken token)
    {
        Result<PosUser> clientInfo;
        if (!request.UserData.Id.HasValue)
            clientInfo = await CreateUserInfo(request.UserData, token);
        else
            clientInfo = await UpdateUserInfo(request.UserData, token);
        return clientInfo;
    }

    private async Task<Result<PosUser>> CreateUserInfo(UserRequest request, CancellationToken token)
    {
        var user = request.Adapt<PosUser>();
        await context.SaveChangesAsync(token);
        context.Users.Add(user);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            return new Result<PosUser>(new EntityExistsException());
        }

        return user;
    }

    private async Task<Result<PosUser>> UpdateUserInfo(UserRequest request, CancellationToken token)
    {
        var user =
            await context.Users.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken: token);

        if (user is null)
            return new Result<PosUser>(new EntityNotFoundException(request.Id.ToString()));

        request.Adapt(user, Config);
        context.Update(user);
        await context.SaveChangesAsync(token);
        return user;
    }
}