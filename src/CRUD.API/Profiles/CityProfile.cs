using AutoMapper;

namespace CRUD.API.Profiles;

/// <summary>AutoMapper profile for <see cref="Entities.City"/> related mappings. </summary>
public class CityProfile: Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CityProfile"/> class and defines city-related mappings.
    /// </summary>
    public CityProfile()
    {
        CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
        CreateMap<Entities.City, Models.CityDto>();
    }
}