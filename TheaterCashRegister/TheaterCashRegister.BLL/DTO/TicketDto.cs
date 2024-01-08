using TheaterCashRegister.DAL.Models;

namespace TheaterCashRegister.BLL.DTO;

public class TicketDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int SeatNumber { get; set; }
    public string Status { get; set; }
    public int PerformanceId { get; set; }
    public BookingDto Booking;
}