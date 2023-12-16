using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TheaterCashRegister.BLL.DTO;
using TheaterCashRegister.BLL.MappingProfiles;
using TheaterCashRegister.BLL.Service;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Models;
using TheaterCashRegister.DAL.Repository;
using TheaterCashRegister.DAL.Repository.IRepository;

namespace TheaterCashRegister.Console.PL;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices(args);
        var ticketService = serviceProvider.GetRequiredService<ITicketService>();
        var performanceService = serviceProvider.GetRequiredService<IPerformanceService>();

        // Create and add a new performance
        var dateTime = DateTime.Now;
        var performanceDto = new PerformanceDto
        {
            Title = "Sample Performance",
            Description = "A sample performance for demonstration",
            Genre = "Genre",
            Author = "Author",
            Date = dateTime
        };
        var addedPerformance = performanceService.AddPerformance(performanceDto);
        System.Console.WriteLine("Added performance:");
        System.Console.WriteLine(JsonConvert.SerializeObject(addedPerformance, Formatting.Indented));

        // Search for performances
        var performancesAuthor = performanceService.SearchPerformances(author: "Author");
        System.Console.WriteLine("\nSearch Performances (by author):");
        System.Console.WriteLine(JsonConvert.SerializeObject(performancesAuthor, Formatting.Indented));
        var performancesTitle = performanceService.SearchPerformances(author: "Non-existing title");
        System.Console.WriteLine("\nSearch Performances (by title):");
        System.Console.WriteLine(JsonConvert.SerializeObject(performancesTitle, Formatting.Indented));
        var performancesGenre = performanceService.SearchPerformances(genre: "Genre");
        System.Console.WriteLine("\nSearch Performances (by genre):");
        System.Console.WriteLine(JsonConvert.SerializeObject(performancesGenre, Formatting.Indented));
        var performancesDate = performanceService.SearchPerformances(date: dateTime);
        System.Console.WriteLine("\nSearch Performances (by date):");
        System.Console.WriteLine(JsonConvert.SerializeObject(performancesDate, Formatting.Indented));

        // Create and add a new ticket related to the performance
        var ticketDto1 = new TicketDto
        {
            Price = 100M,
            SeatNumber = 1,
            Status = "Available",
            PerformanceId = addedPerformance.Id
        };
        var createdTicket1 = ticketService.CreateTicket(ticketDto1);
        System.Console.WriteLine("\nCreated ticket:");
        System.Console.WriteLine(JsonConvert.SerializeObject(createdTicket1, Formatting.Indented));
        var ticketDto2 = new TicketDto
        {
            Price = 100M,
            SeatNumber = 2,
            Status = "Available",
            PerformanceId = addedPerformance.Id
        };
        var createdTicket2 = ticketService.CreateTicket(ticketDto2);
        System.Console.WriteLine("\nCreated ticket:");
        System.Console.WriteLine(JsonConvert.SerializeObject(createdTicket2, Formatting.Indented));
        
        // Get tickets by performance id
        var ticketsByPerformanceId = performanceService.GetPerformanceById(addedPerformance.Id).Tickets;
        System.Console.WriteLine("\nAll tickets for performance id: " + addedPerformance.Id);
        System.Console.WriteLine(JsonConvert.SerializeObject(ticketsByPerformanceId, Formatting.Indented));
        
        // Get a ticket by seatNumber and performanceId
        var fetchedTicket = ticketService.GetTicket(1, addedPerformance.Id);
        System.Console.WriteLine("\nFetched ticket:");
        System.Console.WriteLine(JsonConvert.SerializeObject(fetchedTicket, Formatting.Indented));

        // Sell a ticket
        int seatNumber = 1;
        int performanceId = addedPerformance.Id;
        var soldTicket = ticketService.BuyTicket(seatNumber, performanceId);
        System.Console.WriteLine("\nSold ticket:");
        System.Console.WriteLine(JsonConvert.SerializeObject(soldTicket, Formatting.Indented));

        // Book a ticket
        seatNumber = 2;
        var (bookedTicket, uuid) = ticketService.BookTicket(seatNumber, performanceId);
        System.Console.WriteLine("\nBooked ticket:");
        System.Console.WriteLine(JsonConvert.SerializeObject(bookedTicket, Formatting.Indented));
        System.Console.WriteLine($"Booking UUID: {uuid}");

        // Confirm booked ticket
        var confirmedTicket = ticketService.ConfirmBookedTicket(uuid);
        System.Console.WriteLine("\nConfirmed ticket:");
        System.Console.WriteLine(JsonConvert.SerializeObject(confirmedTicket, Formatting.Indented));

        // Delete performance
        System.Console.WriteLine($"\nDeleting performance with ID {addedPerformance.Id}...");
        performanceService.DeletePerformance(addedPerformance.Id);
        var performances = performanceService.SearchPerformances();
        System.Console.WriteLine("\nPerformances after the deletion:");
        System.Console.WriteLine(JsonConvert.SerializeObject(performances, Formatting.Indented));
    }

    private static IServiceProvider ConfigureServices(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddTransient<ApplicationDbContextFactory, ApplicationDbContextFactory>();

        serviceCollection.AddScoped<ApplicationDbContext>(provider =>
            provider.GetService<ApplicationDbContextFactory>()?.CreateDbContext(args)!);

        serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddTransient<ITicketService, TicketService>();
        serviceCollection.AddTransient<IPerformanceService, PerformanceService>();

        serviceCollection.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<TicketProfile>();
            cfg.AddProfile<PerformanceProfile>();
            cfg.AddProfile<BookingProfile>();
        }, Assembly.GetExecutingAssembly());

        return serviceCollection.BuildServiceProvider();
    }
}