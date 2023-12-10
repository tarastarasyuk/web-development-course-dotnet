namespace TheaterCashRegister.SSR.PL.Models;

public class TicketViewModel
{
    public int Id { get; set; }
    public int PerformanceId { get; set; }
    public int SeatNumber { get; set; }
    public string Status { get; set; }
    public decimal Price { get; set; }
    public BookingViewModel Booking { get; set; }
}