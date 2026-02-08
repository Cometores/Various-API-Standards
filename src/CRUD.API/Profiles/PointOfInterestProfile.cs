using AutoMapper;

namespace CRUD.API.Profiles;

/// <summary>AutoMapper profile for <see cref="Entities.PointOfInterest"/> related mappings. </summary>
public class PointOfInterestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PointOfInterestProfile"/> class and defines point of interest-related mappings.
    /// </summary>
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
        CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
        CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
    }
}