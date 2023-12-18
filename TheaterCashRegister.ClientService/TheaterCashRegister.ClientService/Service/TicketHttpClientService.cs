using TheaterCashRegister.ClientService.DTO;
using TheaterCashRegister.ClientService.Exception;
using TheaterCashRegister.ClientService.Service.IService;
using TheaterCashRegister.ClientService.Util;

namespace TheaterCashRegister.ClientService.Service;

public class TicketHttpClientService : ITicketHttpClientService
{
    private const string BaseTicketApiUrl = "/api/Ticket";
    private const string BuyTicketEndpoint = "/buy";
    private const string BookTicketEndpoint = "/book";
    private const string ConfirmBookTicketEndpoint = "/confirm";
    private const string CreateTicketError = "An error occurred while creating a ticket.";
    private const string RetrieveTicketError = "An error occurred while retrieving a ticket.";
    private const string BuyTicketError = "An error occurred while buying a ticket.";
    private const string BookTicketError = "An error occurred while booking a ticket.";
    private const string ConfirmBookTicketError = "An error occurred while confirming a booked ticket.";

    private readonly HttpClient _httpClient;

    public TicketHttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public TicketDto CreateTicketAsync(CreateTicketRequestDto createTicketRequestDto)
    {
        try
        {
            HttpResponseMessage response = _httpClient.PostAsJsonAsync(BaseTicketApiUrl, createTicketRequestDto)
                .GetAwaiter().GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var ticket = response.Content.ReadAsAsync<TicketDto>().GetAwaiter().GetResult();
            return ticket;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(CreateTicketError, e);
        }
    }

    public TicketDto GetTicketAsync(int seatNumber, int performanceId)
    {
        try
        {
            HttpResponseMessage response = _httpClient.GetAsync($"{BaseTicketApiUrl}/{seatNumber}/{performanceId}")
                .GetAwaiter().GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var ticket = response.Content.ReadAsAsync<TicketDto>().GetAwaiter().GetResult();
            return ticket;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(RetrieveTicketError, e);
        }
    }

    public TicketDto BuyTicketAsync(BuyTicketRequestDto buyTicketRequest)
    {
        try
        {
            HttpResponseMessage response = _httpClient
                .PostAsJsonAsync(BaseTicketApiUrl + BuyTicketEndpoint, buyTicketRequest).GetAwaiter().GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var ticket = response.Content.ReadAsAsync<TicketDto>().GetAwaiter().GetResult();
            return ticket;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(BuyTicketError, e);
        }
    }

    public (TicketDto ticket, string uuid) BookTicketAsync(BookTicketRequestDto bookTicketRequest)
    {
        try
        {
            HttpResponseMessage response = _httpClient
                .PostAsJsonAsync(BaseTicketApiUrl + BookTicketEndpoint, bookTicketRequest).GetAwaiter().GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var result = response.Content.ReadAsAsync<BookTicketResponseDto>().GetAwaiter().GetResult();
            return (result.Ticket, result.BookingId);
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(BookTicketError, e);
        }
    }

    public TicketDto ConfirmBookedTicketAsync(ConfirmationTicketRequestDto confirmationTicketRequest)
    {
        try
        {
            HttpResponseMessage response = _httpClient
                .PostAsJsonAsync(BaseTicketApiUrl + ConfirmBookTicketEndpoint, confirmationTicketRequest).GetAwaiter()
                .GetResult();
            HttpClientUtil.EnsureSuccessStatusCode(response);

            var ticket = response.Content.ReadAsAsync<TicketDto>().GetAwaiter().GetResult();
            return ticket;
        }
        catch (HttpRequestException e)
        {
            throw new TheaterApiException(ConfirmBookTicketError, e);
        }
    }
}