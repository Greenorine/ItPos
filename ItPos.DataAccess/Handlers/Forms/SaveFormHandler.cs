using ItPos.Domain.DTO.V1.Form;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Forms;

public record SaveForm(FormRequest FormData) : IRequest<Form>;

public class SaveFormHandler : IRequestHandler<SaveForm, Form>
{
    private readonly ItPosDbContext context;
    private readonly IMediator mediator;

    public SaveFormHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
    }

    public async Task<Form> Handle(SaveForm request, CancellationToken token)
    {
        Form form;
        if (!request.FormData.Id.HasValue)
            form = await CreateForm(request.FormData, token);
        else
            form = await UpdateForm(request.FormData, token);
        return form;
    }

    private async Task<Form> CreateForm(FormRequest request, CancellationToken token)
    {
        var form = request.Adapt<Form>();
        context.Forms.Add(form);

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            throw new EntityExistsException();
        }

        return form;
    }

    private async Task<Form> UpdateForm(FormRequest request, CancellationToken token)
    {
        var form =
            await context.Forms.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken: token);

        if (form is null)
            throw new EntityNotFoundException(nameof(request.Id), request.Id.ToString());

        request.Adapt(form);
        context.Update(form);
        await context.SaveChangesAsync(token);
        return form;
    }
}