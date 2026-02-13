using AutoMapper;
using Microsoft.AspNetCore.Http.Features;

namespace Testing.API.MapperProfiles
{
    public class StatisticsProfile : Profile
    {
        public StatisticsProfile()
        {
            CreateMap<IHttpConnectionFeature, Models.StatisticsDto>();
        }
    }
}
