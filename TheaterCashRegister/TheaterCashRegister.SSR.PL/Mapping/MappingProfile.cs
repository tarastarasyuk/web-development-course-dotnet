using AutoMapper;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.SSR.PL.Models;

namespace TheaterCashRegister.SSR.PL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PerformanceCreateViewModel, PerformanceDto>();
        CreateMap<PerformanceDto, PerformanceViewModel>();
        CreateMap<TicketCreateViewModel, TicketDto>();
        CreateMap<TicketDto, TicketViewModel>();
        CreateMap<BookingDto, BookingViewModel>();
    }
}