using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;

namespace DataBase.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(des => des.AddressDto, opt => opt.MapFrom(src => src.Address));

        CreateMap<CustomerDto, Customer>()
            .ForMember(des => des.Address, opt => opt.MapFrom(src => src.AddressDto));
    }
}