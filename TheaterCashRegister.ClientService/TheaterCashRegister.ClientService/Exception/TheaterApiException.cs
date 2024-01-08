namespace TheaterCashRegister.ClientService.Exception;

public class TheaterApiException : System.Exception
{
    public TheaterApiException(string message)
        : base(message)
    {
    }

    public TheaterApiException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}