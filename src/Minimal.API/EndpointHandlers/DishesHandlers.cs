using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Minimal.API.Data;
using Minimal.API.Entities;
using Minimal.API.Models;

namespace Minimal.API.EndpointHandlers;

/// <summary>Provides methods for handling CRUD operations on dishes.</summary>
public static class DishesHandlers
{
    /// <summary>Retrieves a collection of dishes from the database, optionally filtering by name.</summary>
    /// <param name="dishesDbContext">The database context used to access the dishes data.</param>
    /// <param name="claimsPrincipal">The claims principal representing the authenticated user.</param>
    /// <param name="mapper">The mapper instance for converting database entities to DTOs.</param>
    /// <param name="logger">The logger instance used for logging diagnostic information.</param>
    /// <param name="name">An optional filter parameter used to search for dishes by name.</param>
    /// <returns>A collection of dishes matching the specified criteria.</returns>
    public static async Task<Ok<IEnumerable<DishDto>>> GetDishesAsync(
        DishesDbContext dishesDbContext,
        ClaimsPrincipal claimsPrincipal,
        IMapper mapper,
        ILogger<DishDto> logger,
        string? name)
    {
        Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

        logger.LogInformation("Getting the dishes");

        return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(
            await dishesDbContext.Dishes
                .Where(d => name == null || d.Name.Contains(name))
                .ToListAsync()));
    }

    /// <summary>Retrieves a dish from the database based on its unique identifier.</summary>
    /// <param name="dishesDbContext">The database context used to access the dishes data.</param>
    /// <param name="mapper">The mapper instance for converting the database entity to a DTO.</param>
    /// <param name="dishId">The unique identifier of the dish to retrieve.</param>
    /// <returns>A result containing the requested dish, or NotFound if no dish with the specified identifier was found.</returns>
    public static async Task<Results<NotFound, Ok<DishDto>>> GetDishByIdAsync(
        DishesDbContext dishesDbContext,
        IMapper mapper,
        Guid dishId)
    {
        var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        
        if (dishEntity == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
    }

    /// <summary>Retrieves a dish from the database by its name.</summary>
    /// <param name="dishesDbContext">The database context used to access the dishes data.</param>
    /// <param name="mapper">The mapper instance for converting the database entity to a DTO.</param>
    /// <param name="dishName">The name of the dish to retrieve.</param>
    /// <returns>A result containing the requested dish, or NotFound if no dish with the specified name was found.</returns>
    public static async Task<Ok<DishDto>> GetDishByNameAsync(
        DishesDbContext dishesDbContext,
        IMapper mapper,
        string dishName)
    {
        return TypedResults.Ok(mapper.Map<DishDto>(
            await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)));
    }

    /// <summary>
    /// Creates a new dish in the database and returns the created dish with its location.
    /// </summary>
    /// <param name="dishesDbContext">The database context used to persist the new dish.</param>
    /// <param name="mapper">The mapper instance for converting between DTOs and entities.</param>
    /// <param name="dishForCreationDto">The DTO containing the details of the dish to be created.</param>
    /// <returns>A response containing the created dish data and its location URI.</returns>
    public static async Task<CreatedAtRoute<DishDto>> CreateDishAsync(
        DishesDbContext dishesDbContext,
        IMapper mapper,
        DishForCreationDto dishForCreationDto)
    {
        var dishEntity = mapper.Map<Dish>(dishForCreationDto);
        dishesDbContext.Add(dishEntity);
        await dishesDbContext.SaveChangesAsync();

        var dishToReturn = mapper.Map<DishDto>(dishEntity);

        return TypedResults.CreatedAtRoute(
            dishToReturn,
            "GetDish",
            new { dishId = dishToReturn.Id });
    }

    /// <summary>Updates an existing dish in the database with the provided data.</summary>
    /// <param name="dishesDbContext">The database context used to access and modify dish data.</param>
    /// <param name="mapper">The mapper instance used to map the update data to the dish entity.</param>
    /// <param name="dishId">The unique identifier of the dish to be updated.</param>
    /// <param name="dishForUpdateDto">The data transfer object containing the updated details of the dish.</param>
    /// <returns>A result indicating whether the update operation was successful, or if the dish was not found.</returns>
    public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(
        DishesDbContext dishesDbContext,
        IMapper mapper,
        Guid dishId,
        DishForUpdateDto dishForUpdateDto)
    {
        var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        
        if (dishEntity == null)
            return TypedResults.NotFound();

        mapper.Map(dishForUpdateDto, dishEntity);
        await dishesDbContext.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    /// <summary>Deletes a dish from the database using the specified dish identifier.</summary>
    /// <param name="dishesDbContext">The database context used to perform operations on the dishes data.</param>
    /// <param name="dishId">The unique identifier of the dish to be deleted.</param>
    /// <returns>A result containing either a NotFound response if the dish does not exist, or a NoContent response upon successful deletion.</returns>
    public static async Task<Results<NotFound, NoContent>> DeleteDishAsync(
        DishesDbContext dishesDbContext,
        Guid dishId)
    {
        var dishEntity = await dishesDbContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        
        if (dishEntity == null)
            return TypedResults.NotFound();

        dishesDbContext.Dishes.Remove(dishEntity);
        await dishesDbContext.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}