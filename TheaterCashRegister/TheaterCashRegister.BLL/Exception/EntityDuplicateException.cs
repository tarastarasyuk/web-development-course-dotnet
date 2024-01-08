namespace TheaterCashRegister.BLL.Exception;

public class EntityDuplicateException : System.Exception
{
    public EntityDuplicateException(string message)
        : base(message)
    {
    }
}