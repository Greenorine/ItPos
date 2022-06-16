using System.Reflection;
using ItPos.Api.Extensions;
using ItPos.DataAccess;
using ItPos.Domain.Models.Response;
using ItPos.Infrastructure.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, config) =>
{
    config.MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddControllersWithErrorHandlers();

builder.Services.AddSwagger();

builder.Services.AddSingleton<ITokenManager, JwtTokenManager>();
builder.Services.AddJwtSecurity(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddMapster();
builder.Services.AddMediatR(typeof(AssemblyDummy).GetTypeInfo().Assembly);
builder.Services.AddDbModel(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
    var response = new ResponseError(exception?.Message ?? "");
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

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