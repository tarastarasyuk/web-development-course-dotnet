namespace TheaterCashRegister.SSR.PL.Models;

public class TicketCreateViewModel
{
    public int PerformanceId { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
}