using System.Reflection;
using System.Text;
using CRUD.API.Data;
using CRUD.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CRUD.API;

/// <summary>Provides extension methods for configuring and setting up dependency injection.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers dependency injection services across all application layers used by this program.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method is intended to be called during startup (e.g., in <c>Program.cs</c>) to keep DI registration organized.
    /// </remarks>
    public static IServiceCollection AddProgramDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddWebDependencies(configuration)
            .AddDataLayer(configuration)
            .AddSingleton<FileExtensionContentTypeProvider>()
            .AddMailService();
    }

    private static IServiceCollection AddWebDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(setupAction =>
        {
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            setupAction.IncludeXmlComments(xmlCommentsFullPath);

            setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Input a valid token to access this API"
            });

            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "CityInfoApiBearerAuth"
                        }
                    },
                    new List<string>()
                }
            });
        });

        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidAudience = configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("MustBeFromAntwerp", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("city", "Antwerp");
            });
        });

        services.AddApiVersioning(setupAction =>
        {
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            setupAction.ReportApiVersions = true;
        });

        return services;
    }

    private static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        /* Dummy data */
        services.AddSingleton<CitiesInMemoryDataStore>();

        /* SQLite Database */
        // will register with a scoped lifetime
        services.AddDbContext<CitiesDbContext>(dbContextOptions => dbContextOptions.UseSqlite(
            configuration["ConnectionStrings:CityInfoDBConnectionString"]));

        services.AddScoped<ICityInfoRepository, CityInfoRepository>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static IServiceCollection AddMailService(this IServiceCollection services)
    {
        /* Mail service for concrete build */
#if DEBUG
        services.AddTransient<IMailService, LocalMailService>();
#else
        builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

        return services;
    }
}