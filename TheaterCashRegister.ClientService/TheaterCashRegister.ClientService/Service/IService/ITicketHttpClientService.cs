using TheaterCashRegister.ClientService.DTO;

namespace TheaterCashRegister.ClientService.Service.IService;

public interface ITicketHttpClientService
{
    TicketDto CreateTicketAsync(CreateTicketRequestDto createTicketRequestDto);
    TicketDto GetTicketAsync(int seatNumber, int performanceId);
    TicketDto BuyTicketAsync(BuyTicketRequestDto buyTicketRequest);
    (TicketDto ticket, string uuid) BookTicketAsync(BookTicketRequestDto bookTicketRequest);
    TicketDto ConfirmBookedTicketAsync(ConfirmationTicketRequestDto confirmationTicketRequest);
}