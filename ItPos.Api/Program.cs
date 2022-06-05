using System.Reflection;
using ItPos.Api.Extensions;
using ItPos.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithErrorHandlers();

builder.Services.AddSwagger();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatR(typeof(AssemblyDummy).GetTypeInfo().Assembly);
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ItPosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("pos_connection")!,
        opts => opts.MigrationsAssembly("ItPos.Api"));
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ItPosDbContext>();
    context.Database.Migrate();
    /*context.Applications.Add(new Application
    {
        FormId = Guid.NewGuid(),
        OwnerId = Guid.NewGuid(),
        Stage = new FormStage
        {
            Index = 0,
            Name = "В обработке",
            Permission = "помощь.в.обработке"
        },
        Inputs = new List<FormInput>
        {
            new()
            {
                Name = "Ссылка на чек",
                Value = "https://google.com"
            }
        }
    });*/

    context.SaveChanges();
}

app.Run();