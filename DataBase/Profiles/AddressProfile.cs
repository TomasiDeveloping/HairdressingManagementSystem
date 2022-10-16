using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;

namespace DataBase.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<AddressDto, Address>();
    }
}