using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Forms;

public record GetFormById(Guid Id) : IRequest<Form>;

public class GetFormByIdHandler : IRequestHandler<GetFormById, Form>
{
    private readonly ItPosDbContext context;

    public GetFormByIdHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<Form> Handle(GetFormById request,
        CancellationToken cancellationToken)
    {
        var form =
            await context.Forms.AsNoTracking().Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return form ?? throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());
    }
}