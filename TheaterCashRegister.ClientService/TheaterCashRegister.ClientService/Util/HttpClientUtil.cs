using TheaterCashRegister.ClientService.Exception;

namespace TheaterCashRegister.ClientService.Util;

public class HttpClientUtil
{
    public static void EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage;

            if (response.Content != null)
            {
                errorMessage = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                errorMessage = "An error occurred while communicating with the API.";
            }

            throw new TheaterApiException(errorMessage);
        }
    }
}