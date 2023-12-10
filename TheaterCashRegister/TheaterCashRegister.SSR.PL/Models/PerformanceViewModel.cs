namespace TheaterCashRegister.SSR.PL.Models;

public class PerformanceViewModel
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<TicketViewModel> Tickets { get; set; }
}