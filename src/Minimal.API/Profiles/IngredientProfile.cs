using AutoMapper;
using Minimal.API.Entities;
using Minimal.API.Models;

namespace Minimal.API.Profiles;

/// <summary>AutoMapper profile for <see cref="Entities.Ingredient"/> related mappings. </summary>
public class IngredientProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IngredientProfile"/> class and defines ingredient-related mappings.
    /// </summary>
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDto>()
            .ForMember(
                d => d.DishId,
                o => o.MapFrom(s => s.Dishes.First().Id));
    }
}