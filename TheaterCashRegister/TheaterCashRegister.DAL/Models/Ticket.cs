using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterCashRegister.DAL.Models;

public enum TicketStatus
{
    Available,
    Booked,
    Sold
}

public class Ticket
{
    [Key] public int Id { get; set; }
    [Required] public decimal Price { get; set; }
    [Required] public int SeatNumber { get; set; }
    [Required] public TicketStatus Status { get; set; }

    [ForeignKey("Performance")] public int PerformanceId { get; set; }
    public Performance Performance { get; set; }
    public virtual Booking Booking { get; set; }
}