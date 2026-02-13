using Minimal.API.EndpointFilters;
using Minimal.API.EndpointHandlers;
using Minimal.API.Models;

namespace Minimal.API.Extensions;

/// <summary>
/// Provides extension methods for registering endpoints in the application's <see cref='IEndpointRouteBuilder'/>.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Registers the endpoint routes for dish-related operations, including creating, retrieving, updating,
    /// and deleting dishes, as well as applying authorization and filtering rules.
    /// </summary>
    /// <param name="endpointRouteBuilder">The route builder used to define and map the dish-related endpoints.</param>
    public static void RegisterDishesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // Group endpoints together
        var dishesEndpoints = endpointRouteBuilder.MapGroup("/dishes").RequireAuthorization();
        var dishWithGuidIdEndpoints = dishesEndpoints.MapGroup("/{dishId:guid}");
        var dishWithGuidIdEndpointsAndLockFilters = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}")
            .RequireAuthorization()
            .AddEndpointFilter(new DishIsLockedFilter(new("fd630a57-2352-4731-b25c-db9cc7601b16")));

        // Map endpoints
        dishesEndpoints.MapGet("", DishesHandlers.GetDishesAsync);
        
        dishWithGuidIdEndpoints.MapGet("", DishesHandlers.GetDishByIdAsync)
            .WithName("GetDish")
            .WithOpenApi()
            .WithSummary("Get a dish by providing an id.")
            .WithDescription("Dishes are identified by a URI containing a dish identifier. This identifier is a GUID.");
        
        dishesEndpoints.MapGet("/{dishName}", DishesHandlers.GetDishByNameAsync)
            .AllowAnonymous()
            .WithOpenApi(operation =>
            {
                operation.Deprecated = true;
                return operation;
            });
        
        dishesEndpoints.MapPost("", DishesHandlers.CreateDishAsync)
            .RequireAuthorization("RequireAdminFromGermany")
            .AddEndpointFilter<ValidateAnnotationsFilter>()
            .ProducesValidationProblem(400)
            .Accepts<DishForCreationDto>(
                "application/json",
                "application/vnd.marvin.dishforcreation+json");
        
        dishWithGuidIdEndpointsAndLockFilters.MapPut("", DishesHandlers.UpdateDishAsync);
        
        dishWithGuidIdEndpointsAndLockFilters.MapDelete("", DishesHandlers.DeleteDishAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();
    }

    /// <summary>
    /// Registers the endpoint routes for ingredient-related operations, allowing for the retrieval
    /// of ingredients for a specified dish. Includes authorization requirements.
    /// </summary>
    /// <param name="endpointRouteBuilder">The route builder used to define and map the ingredient-related endpoints.</param>
    public static void RegisterIngredientsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}/ingredients")
            .RequireAuthorization();

        ingredientsEndpoints.MapGet("", IngredientsHandlers.GetIngredientsAsync);
    }
}