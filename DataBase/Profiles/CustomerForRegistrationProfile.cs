using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Models;

namespace DataBase.Profiles;

public class CustomerForRegistrationProfile : Profile
{
    public CustomerForRegistrationProfile()
    {
        CreateMap<CustomerForRegistrationDto, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<CustomerForRegistrationDto, CustomerDto>()
            .ForMember(des => des.AddressDto, opt => opt.MapFrom(src => src.Address));
    }
}