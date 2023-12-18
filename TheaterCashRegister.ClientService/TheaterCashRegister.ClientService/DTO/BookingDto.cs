namespace TheaterCashRegister.ClientService.DTO;

public class BookingDto
{
    public int TicketId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Guid UUID { get; set; }
}