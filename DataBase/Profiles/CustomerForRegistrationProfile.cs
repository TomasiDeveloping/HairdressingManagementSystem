using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Models;

namespace DataBase.Profiles;

public class CustomerForRegistrationProfile : Profile
{
    public CustomerForRegistrationProfile()
    {
        CreateMap<CustomerForRegistration, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<CustomerForRegistration, CustomerDto>()
            .ForMember(des => des.AddressDto, opt => opt.MapFrom(src => src.Address));
    }
}