using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Minimal.API.Data;
using Minimal.API.Extensions;

namespace Minimal.API;

/// <summary>
/// Provides extension methods for configuring and setting up dependency injection and Middleware registration.
/// </summary>
public static class StartupHelperExtensions
{
    /// <summary>Registers dependency injection services across all application layers used by this program.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method is intended to be called during startup (e.g., in <c>Program.cs</c>) to keep DI registration organized.
    /// </remarks>
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddWebDependencies(configuration)
            .AddDataLayer(configuration);
    }

    private static IServiceCollection AddWebDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProblemDetails();
        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();
        services.AddAuthorizationBuilder().AddPolicy("RequireAdminFromGermany", policy =>
            policy.RequireRole("admin").RequireClaim("country", "Germany"));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("TokenAuthNZ", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Token-based authentication and authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "TokenAuthNZ"
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // register the DbContext on the container, getting the connection string from appSettings   
        services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
            configuration["ConnectionStrings:DishesDBConnectionString"]));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();
        app.RegisterDishesEndpoints();
        app.RegisterIngredientsEndpoints();
        
        return app;
    }
}