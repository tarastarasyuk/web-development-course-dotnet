using System.ComponentModel.DataAnnotations;

namespace TheaterCashRegister.DAL.Models;

public class Performance
{
    [Key] public int Id { get; set; }
    [Required] public string Title { get; set; }
    public string? Description { get; set; }
    [Required] public string Genre { get; set; }
    [Required] public string Author { get; set; }
    [Required] public DateTime Date { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; }
}