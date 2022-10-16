using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;

namespace DataBase.Profiles;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(des => des.CustomerDto, opt => opt.MapFrom(src => src.Customer))
            .ForMember(des => des.EmployeeDto, opt => opt.MapFrom(src => src.Employee));

        CreateMap<AppointmentDto, Appointment>()
            .ForMember(des => des.Customer, opt => opt.MapFrom(src => src.CustomerDto))
            .ForMember(des => des.Employee, opt => opt.MapFrom(src => src.EmployeeDto));
    }
}