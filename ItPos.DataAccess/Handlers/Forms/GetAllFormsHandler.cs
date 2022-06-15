using ItPos.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Forms;

public record GetAllForms : IRequest<List<Form>>;

public class GetAllFormsHandler : IRequestHandler<GetAllForms, List<Form>>
{
    private readonly ItPosDbContext context;

    public GetAllFormsHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Form>> Handle(GetAllForms request,
        CancellationToken cancellationToken)
    {
        return await context.Forms.AsNoTracking().Where(c => !c.IsDeleted)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}