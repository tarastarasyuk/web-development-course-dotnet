using System.ComponentModel.DataAnnotations;

namespace TheaterCashRegister.BLL.DTO;

public class PerformanceDto
{
    public int Id { get; set; }
    [Display(Name = "Performance title")]
    public string Title { get; set; }
    [Display(Name = "Performance description")]
    public string Description { get; set; }
    [Display(Name = "Performance genre")]
    public string Genre { get; set; }
    [Display(Name = "Performance author")]
    public string Author { get; set; }
    [Display(Name = "Performance date")]
    public DateTime Date { get; set; }
    [Display(Name = "Performance tickets")]
    public IEnumerable<TicketDto> Tickets { get; set; }
}