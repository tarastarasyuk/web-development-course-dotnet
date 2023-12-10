using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.SSR.PL.Models;

namespace TheaterCashRegister.SSR.PL.Controllers;

public class PerformanceController : Controller
{
    private readonly IPerformanceService _performanceService;
    private readonly IMapper _mapper;

    public PerformanceController(IPerformanceService performanceService, IMapper mapper)
    {
        _performanceService = performanceService;
        _mapper = mapper;
    }

    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(string? title, string? author, string? genre, DateTime? date)
    {
        var performances = _performanceService.SearchPerformances(author: author, title: title, genre: genre, date: date);
        return View(performances);  
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PerformanceCreateViewModel model)
    {
        var performanceDto = _mapper.Map<PerformanceDto>(model);
        _performanceService.AddPerformance(performanceDto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var performanceDto = _performanceService.GetPerformanceById(id);
        var performanceViewModel = _mapper.Map<PerformanceViewModel>(performanceDto);
        return View(performanceViewModel);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _performanceService.DeletePerformance(id);
        return RedirectToAction("Index");
    }
}