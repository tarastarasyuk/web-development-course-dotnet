namespace TheaterCashRegister.SSR.PL.Models;

public class BookingViewModel
{
    public int TicketId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Guid UUID { get; set; }
}