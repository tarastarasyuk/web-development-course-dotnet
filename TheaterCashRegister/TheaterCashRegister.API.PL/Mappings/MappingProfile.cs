using AutoMapper;
using TheaterCashRegister.API.PL.DTO;
using TheaterCashRegister.BLL.DTO;

namespace TheaterCashRegister.API.PL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateTicketRequestDto, TicketDto>();
        CreateMap<CreatePerformanceRequestDto, PerformanceDto>();
    }
}