namespace TheaterCashRegister.ClientService.DTO;

public class BuyTicketRequestDto
{
    public int SeatNumber { get; set; }
    public int PerformanceId { get; set; }
}