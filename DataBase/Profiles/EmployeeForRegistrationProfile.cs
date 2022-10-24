using AutoMapper;
using Core.Entities.Models;
using Core.Models;

namespace DataBase.Profiles;

public class EmployeeForRegistrationProfile : Profile
{
    public EmployeeForRegistrationProfile()
    {
        CreateMap<EmployeeForRegistration, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<EmployeeForRegistration, Employee>()
            .ForMember(des => des.Address, opt => opt.MapFrom(src => src.AddressDto));
    }
}