using System.Reflection;
using System.Text;
using ItPos.DataAccess;
using ItPos.Domain.Models.Response;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AssemblyDummy = ItPos.Domain.AssemblyDummy;

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
                    if (context.ModelState.ToList().Exists(x => x.Key == string.Empty))
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
    
    public static void AddMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(AssemblyDummy).Assembly);
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);
    }
    
    public static void AddJwtSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!))
                };
            });
    }
    
    public static void AddDbModel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ItPosDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("pos_connection")!,
                opts => opts.MigrationsAssembly("ItPos.Api"));
        });
    }
    
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ItPos API v1"
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
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        }
                    },
                    Array.Empty<string>()
                }
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swagger.IncludeXmlComments(xmlPath);
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }
}