using AutoMapper;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.BLL.MappingProfiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingDto>();
    }
}