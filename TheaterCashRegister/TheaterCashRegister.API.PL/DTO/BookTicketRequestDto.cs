using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheaterCashRegister.API.PL.DTO;

public class BookTicketRequestDto
{
    [Required]
    [Description("Seat number for the ticket.")]
    public int SeatNumber { get; set; }

    [Required]
    [Description("ID of the performance.")]
    public int PerformanceId { get; set; }
}