using TheaterCashRegister.BLL.DTO;

namespace TheaterCashRegister.BLL.Service.IService;

public interface ITicketService
{
    TicketDto CreateTicket(TicketDto ticketDto);
    TicketDto GetTicket(int seatNumber, int performanceId);
    TicketDto BuyTicket(int seatNumber, int performanceId);
    (TicketDto, Guid) BookTicket(int seatNumber, int performanceId);
    TicketDto ConfirmBookedTicket(Guid uuid);
}