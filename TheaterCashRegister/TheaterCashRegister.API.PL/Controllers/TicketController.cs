using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheaterCashRegister.API.PL.DTO;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Service.IService;

namespace TheaterCashRegister.API.PL.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Manage tickets.")]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly IMapper _mapper;

    public TicketController(ITicketService ticketService, IMapper mapper)
    {
        _ticketService = ticketService;
        _mapper = mapper;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new ticket.")]
    public ActionResult<TicketDto> CreateTicket(CreateTicketRequestDto createTicketRequestDto)
    {
        var ticketDto = _mapper.Map<TicketDto>(createTicketRequestDto);
        var createdTicketDto = _ticketService.CreateTicket(ticketDto);

        return CreatedAtAction(nameof(GetTicket),
            new { seatNumber = createdTicketDto.SeatNumber, performanceId = createdTicketDto.PerformanceId },
            createdTicketDto);
    }

    [HttpGet("{seatNumber}/{performanceId}")]
    [SwaggerOperation(Summary = "Retrieves ticket.")]
    public ActionResult<TicketDto> GetTicket(int seatNumber, int performanceId)
    {
        var ticketDto = _ticketService.GetTicket(seatNumber, performanceId);
        return Ok(ticketDto);
    }

    [HttpPost("buy")]
    [SwaggerOperation(Summary = "Buy ticket.")]
    public IActionResult BuyTicket(BuyTicketRequestDto buyTicketRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var boughtTicketDto = _ticketService.BuyTicket(buyTicketRequest.SeatNumber, buyTicketRequest.PerformanceId);
        return Ok(boughtTicketDto);
    }

    [HttpPost("book")]
    [SwaggerOperation(Summary = "Book ticket.")]
    public IActionResult BookTicket(BookTicketRequestDto bookTicketRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (ticketDto, uuid) =
            _ticketService.BookTicket(bookTicketRequest.SeatNumber, bookTicketRequest.PerformanceId);
        return Ok(new { Ticket = ticketDto, BookingId = uuid });
    }

    [HttpPost("confirm")]
    [SwaggerOperation(Summary = "Confirm booked ticket.")]
    public IActionResult ConfirmBookedTicket(ConfirmationTicketRequestDto confirmationTicketRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var confirmedTicketDto = _ticketService.ConfirmBookedTicket(confirmationTicketRequest.Uuid);
        return Ok(confirmedTicketDto);
    }
}