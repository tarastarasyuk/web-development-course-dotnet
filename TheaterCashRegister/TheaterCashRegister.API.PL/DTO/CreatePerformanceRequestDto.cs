using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheaterCashRegister.API.PL.DTO;

public class CreatePerformanceRequestDto
{
    [Required]
    [Description("Title of the performance.")]
    public string Title { get; set; }

    [Description("Description of the performance.")]
    public string Description { get; set; }

    [Required]
    [Description("Genre of the performance.")]
    public string Genre { get; set; }

    [Required]
    [Description("Author of the performance.")]
    public string Author { get; set; }

    [Required]
    [Description("Date of the performance.")]
    public DateTime Date { get; set; }
}