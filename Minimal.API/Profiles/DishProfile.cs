using AutoMapper;
using DishesAPI.Entities;
using Minimal.API.Models;

namespace Minimal.API.Profiles;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<Dish, DishDto>();
        CreateMap<DishForCreationDto, Dish>();
        CreateMap<DishForUpdateDto, Dish>();
    }
}