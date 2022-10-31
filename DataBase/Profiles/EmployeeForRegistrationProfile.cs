using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Models;

namespace DataBase.Profiles;

public class EmployeeForRegistrationProfile : Profile
{
    public EmployeeForRegistrationProfile()
    {
        CreateMap<EmployeeForRegistrationDto, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.WorkEmail))
            .ForMember(des => des.Email, opt => opt.MapFrom(src => src.WorkEmail));


        CreateMap<EmployeeForRegistrationDto, EmployeeDto>()
            .ForMember(des => des.AddressDto, opt => opt.MapFrom(src => src.AddressDto));
    }
}