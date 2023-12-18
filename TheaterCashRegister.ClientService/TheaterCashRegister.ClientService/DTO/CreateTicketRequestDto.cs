namespace TheaterCashRegister.ClientService.DTO;

public class CreateTicketRequestDto
{
    public int SeatNumber { get; set; }
    public int PerformanceId { get; set; }
    public decimal Price { get; set; }
}