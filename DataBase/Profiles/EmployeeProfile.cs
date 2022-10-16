using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;

namespace DataBase.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(des => des.AddressDto, opt => opt.MapFrom(src => src.Address));

        CreateMap<EmployeeDto, Employee>()
            .ForMember(des => des.User, opt => opt.Ignore())
            .ForMember(des => des.Address, opt => opt.MapFrom(src => src.AddressDto));
    }
}