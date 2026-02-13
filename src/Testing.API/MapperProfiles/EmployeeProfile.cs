using AutoMapper;
using Testing.API.DataAccess.Entities;

namespace Testing.API.MapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        { 
            CreateMap<InternalEmployee, Models.InternalEmployeeDto>(); 
        }
    }
}
