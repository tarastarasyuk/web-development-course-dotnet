using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheaterCashRegister.SSR.PL.Models;

namespace TheaterCashRegister.SSR.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string message)
    {
        return View("Error", model: message);
    }
}