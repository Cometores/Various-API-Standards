using AutoMapper;
using DishesAPI.Entities;
using Minimal.API.Models;

namespace Minimal.API.Profiles;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDto>()
            .ForMember(
                d => d.DishId,
                o => o.MapFrom(s => s.Dishes.First().Id));
    }
}