namespace TheaterCashRegister.BLL.Exception;

public class EntityNotFoundException : System.Exception
{
    public EntityNotFoundException(string message)
        : base(message)
    {
    }
}