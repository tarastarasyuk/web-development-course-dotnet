using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterCashRegister.DAL.Models;

public class Booking
{
    [Key] public int Id { get; set; }

    [Required] [ForeignKey("Ticket")] public int TicketId { get; set; }

    [Required] public DateTime ExpirationDate { get; set; }

    [Required] public Guid UUID { get; set; }

    public virtual Ticket Ticket { get; set; }
}