using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TheaterCashRegister.ClientService.DTO;
using TheaterCashRegister.ClientService.Service;
using TheaterCashRegister.ClientService.Service.IService;

namespace TheaterCashRegister.ClientService;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices(args);
        var ticketHttpClientService = serviceProvider.GetRequiredService<ITicketHttpClientService>();
        var performanceHttpClientService = serviceProvider.GetRequiredService<IPerformanceHttpClientService>();

        // Create and add a new performance
        var dateTime = DateTime.Now;
        var createPerformanceRequestDto = new CreatePerformanceRequestDto
        {
            Title = "Sample Performance12",
            Description = "A sample performance for demonstration",
            Genre = "Genre",
            Author = "Author",
            Date = dateTime
        };
        var addedPerformance = performanceHttpClientService.CreatePerformanceAsync(createPerformanceRequestDto);
        Console.WriteLine("Added performance:");
        Console.WriteLine(JsonConvert.SerializeObject(addedPerformance, Formatting.Indented));

        // Search for performances
        var performancesAuthor = performanceHttpClientService.GetPerformancesAsync(author: "Author");
        Console.WriteLine("\nSearch Performances (by author):");
        Console.WriteLine(JsonConvert.SerializeObject(performancesAuthor, Formatting.Indented));
        var performancesTitle = performanceHttpClientService.GetPerformancesAsync(author: "Non-existing title");
        Console.WriteLine("\nSearch Performances (by title):");
        Console.WriteLine(JsonConvert.SerializeObject(performancesTitle, Formatting.Indented));
        var performancesGenre = performanceHttpClientService.GetPerformancesAsync(genre: "Genre");
        Console.WriteLine("\nSearch Performances (by genre):");
        Console.WriteLine(JsonConvert.SerializeObject(performancesGenre, Formatting.Indented));
        var performancesDate = performanceHttpClientService.GetPerformancesAsync(date: dateTime);
        Console.WriteLine("\nSearch Performances (by date):");
        Console.WriteLine(JsonConvert.SerializeObject(performancesDate, Formatting.Indented));

        // Create and add a new ticket related to the performance
        var ticketDto1 = new CreateTicketRequestDto
        {
            Price = 100M,
            SeatNumber = 1,
            PerformanceId = addedPerformance.Id
        };
        var createdTicket1 = ticketHttpClientService.CreateTicketAsync(ticketDto1);
        Console.WriteLine("\nCreated ticket:");
        Console.WriteLine(JsonConvert.SerializeObject(createdTicket1, Formatting.Indented));
        var ticketDto2 = new CreateTicketRequestDto
        {
            Price = 100M,
            SeatNumber = 2,
            PerformanceId = addedPerformance.Id
        };
        var createdTicket2 = ticketHttpClientService.CreateTicketAsync(ticketDto2);
        Console.WriteLine("\nCreated ticket:");
        Console.WriteLine(JsonConvert.SerializeObject(createdTicket2, Formatting.Indented));

        // Get tickets by performance id
        var ticketsByPerformanceId = performanceHttpClientService.GetPerformanceAsync(addedPerformance.Id).Tickets;
        Console.WriteLine("\nAll tickets for performance id: " + addedPerformance.Id);
        Console.WriteLine(JsonConvert.SerializeObject(ticketsByPerformanceId, Formatting.Indented));

        // Get a ticket by seatNumber and performanceId
        var fetchedTicket = ticketHttpClientService.GetTicketAsync(1, addedPerformance.Id);
        Console.WriteLine("\nFetched ticket:");
        Console.WriteLine(JsonConvert.SerializeObject(fetchedTicket, Formatting.Indented));

        // Sell a ticket
        int seatNumber = 1;
        int performanceId = addedPerformance.Id;
        var buyTicketRequest = new BuyTicketRequestDto
        {
            SeatNumber = seatNumber,
            PerformanceId = addedPerformance.Id
        };
        var soldTicket = ticketHttpClientService.BuyTicketAsync(buyTicketRequest);
        Console.WriteLine("\nSold ticket:");
        Console.WriteLine(JsonConvert.SerializeObject(soldTicket, Formatting.Indented));

        // Book a ticket
        seatNumber = 2;
        var bookTicketRequest = new BookTicketRequestDto()
        {
            SeatNumber = seatNumber,
            PerformanceId = addedPerformance.Id
        };
        var (bookedTicket, uuid) = ticketHttpClientService.BookTicketAsync(bookTicketRequest);
        Console.WriteLine("\nBooked ticket:");
        Console.WriteLine(JsonConvert.SerializeObject(bookedTicket, Formatting.Indented));
        Console.WriteLine($"Booking UUID: {uuid}");

        // Confirm booked ticket
        var confirmationTicketRequest = new ConfirmationTicketRequestDto
        {
            Uuid = new Guid(uuid)
        };
        var confirmedTicket = ticketHttpClientService.ConfirmBookedTicketAsync(confirmationTicketRequest);
        Console.WriteLine("\nConfirmed ticket:");
        Console.WriteLine(JsonConvert.SerializeObject(confirmedTicket, Formatting.Indented));

        // Delete performance
        Console.WriteLine($"\nDeleting performance with ID {addedPerformance.Id}...");
        performanceHttpClientService.DeletePerformanceAsync(addedPerformance.Id);
        var performances = performanceHttpClientService.GetPerformancesAsync();
        Console.WriteLine("\nPerformances after the deletion:");
        Console.WriteLine(JsonConvert.SerializeObject(performances, Formatting.Indented));
    }

    private static IServiceProvider ConfigureServices(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var theaterApi = configuration["TheaterApi"];

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient<IPerformanceHttpClientService, PerformanceHttpClientService>(client =>
        {
            client.BaseAddress = new Uri(theaterApi);
        });
        serviceCollection.AddHttpClient<ITicketHttpClientService, TicketHttpClientService>(client =>
        {
            client.BaseAddress = new Uri(theaterApi);
        });

        return serviceCollection.BuildServiceProvider();
    }
}