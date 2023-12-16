using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheaterCashRegister.API.PL.DTO;

public class ConfirmationTicketRequestDto
{
    [Required]
    [Description("UUID of the booked ticket.")]
    public Guid Uuid { get; set; }
}