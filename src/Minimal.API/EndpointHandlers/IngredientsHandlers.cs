using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Minimal.API.Data;
using Minimal.API.Models;

namespace Minimal.API.EndpointHandlers;

/// <summary> Provides methods for handling CRUD operations on ingredients.</summary>
public static class IngredientsHandlers
{
    /// <summary>Asynchronously retrieves the list of ingredients for a specific dish. </summary>
    /// <param name="dishesDbContext">The database context used for accessing dishes and ingredients.</param>
    /// <param name="mapper">The mapper used to convert entities to DTOs.</param>
    /// <param name="dishId">The unique identifier of the dish whose ingredients are to be retrieved.</param>
    /// <returns>A collection of ingredients associated with the specified dish.</returns>
    public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDto>>>> GetIngredientsAsync(
        DishesDbContext dishesDbContext,
        IMapper mapper,
        Guid dishId)
    {
        var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

        if (dishEntity == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDto>>((
            await dishesDbContext.Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == dishId))
            ?.Ingredients));
    }
}