using ItPos.Domain.DTO.V1.Application;
using ItPos.Domain.Exceptions;
using ItPos.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Applications;

public record PublishApplication(ApplicationRequest ApplicationData) : IRequest<Application>;

public class PublishApplicationHandler : IRequestHandler<PublishApplication, Application>
{
    private readonly ItPosDbContext context;
    private readonly IMediator mediator;

    public PublishApplicationHandler(ItPosDbContext context, IMediator mediator)
    {
        this.context = context;
        this.mediator = mediator;
    }

    public async Task<Application> Handle(PublishApplication request, CancellationToken token)
    {
        var application = request.ApplicationData.Adapt<Application>();
        context.Applications.Add(application);

        if (!context.Forms.AsNoTracking().Any(x => x.Id == request.ApplicationData.FormId))
            throw new FormNotFoundException(request.ApplicationData.FormId.ToString());

        try
        {
            await context.SaveChangesAsync(token);
        }
        catch (DbUpdateException ex)
        {
            throw new EntityExistsException();
        }

        return application;
    }
}