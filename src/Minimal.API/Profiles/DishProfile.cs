using AutoMapper;
using Minimal.API.Entities;
using Minimal.API.Models;

namespace Minimal.API.Profiles;

/// <summary>AutoMapper profile for <see cref="Entities.Dish"/> related mappings. </summary>
public class DishProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DishProfile"/> class and defines dish-related mappings.
    /// </summary>
    public DishProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<DishForCreationDto, Dish>();
        CreateMap<DishForUpdateDto, Dish>();
    }
}