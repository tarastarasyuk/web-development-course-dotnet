namespace TheaterCashRegister.ClientService.DTO;

public class CreatePerformanceRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; }
}