using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.SSR.PL.Models;

namespace TheaterCashRegister.SSR.PL.Controllers;

public class TicketController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly IPerformanceService _performanceService;
    private readonly IMapper _mapper;

    public TicketController(ITicketService ticketService, IPerformanceService performanceService, IMapper mapper)
    {
        _ticketService = ticketService;
        _performanceService = performanceService;
        _mapper = mapper;
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        var performancesDto = _performanceService.SearchPerformances();
        var performances = performancesDto.Select(p => new
        {
            Id = p.Id,
            Title = p.Title
        });
        ViewBag.Performances = new SelectList(performances, "Id", "Title");
        return View();
    }

    [HttpPost]
    public IActionResult Create(TicketCreateViewModel model)
    {
        var ticketDto = _mapper.Map<TicketDto>(model);
        _ticketService.CreateTicket(ticketDto);
        return RedirectToAction("Index", "Performance");
    }

    [HttpGet]
    public IActionResult Details(int seatNumber, int performanceId)
    {
        var ticketDto = _ticketService.GetTicket(seatNumber, performanceId);
        var ticketViewModel = _mapper.Map<TicketViewModel>(ticketDto);
        return View(ticketViewModel);
    }


    [HttpPost]
    public IActionResult Buy(int seatNumber, int performanceId)
    {
        var result = _ticketService.BuyTicket(seatNumber, performanceId);
        return RedirectToAction("Details", "Performance", new { id = performanceId });
    }

    [HttpPost]
    public IActionResult Book(int seatNumber, int performanceId)
    {
        var (ticket, uuid) = _ticketService.BookTicket(seatNumber, performanceId);
        return RedirectToAction("Details", "Performance", new { id = performanceId });
    }

    [HttpPost]
    public IActionResult ConfirmBookedTicket(Guid uuid, int performanceId)
    {
        var result = _ticketService.ConfirmBookedTicket(uuid);
        return RedirectToAction("Details", "Performance", new { id = performanceId });
    }
}