using System.Reflection;
using ItPos.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;

namespace ItPos.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddControllersWithErrorHandlers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    if (context.ModelState.Exists(x => x.Key == string.Empty))
                        return new BadRequestObjectResult(new ResponseError("Request should be not empty"));
                    var errors = context.ModelState
                        .Where(kv => kv.Value?.ValidationState is ModelValidationState.Invalid && kv.Value.Errors.Any())
                        .Select(kv => kv.Value!.Errors.FirstOrDefault()!.ErrorMessage).ToList();
                    return errors.Exists(e =>
                        e.Contains(", line", StringComparison.OrdinalIgnoreCase) &&
                        e.Contains(", position", StringComparison.OrdinalIgnoreCase))
                        ? new BadRequestObjectResult(new ResponseError("Invalid request"))
                        : new BadRequestObjectResult(new ResponseError(errors));
                };
            });
    }
    
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "POS Client API v1"
            });

            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}