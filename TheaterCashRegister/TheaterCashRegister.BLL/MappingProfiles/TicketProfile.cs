using AutoMapper;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.BLL.MappingProfiles;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<Ticket, TicketDto>();
        CreateMap<TicketDto, Ticket>();
    }
}