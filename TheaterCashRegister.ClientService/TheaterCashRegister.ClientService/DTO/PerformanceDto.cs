namespace TheaterCashRegister.ClientService.DTO;

public class PerformanceDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<TicketDto> Tickets { get; set; }
}