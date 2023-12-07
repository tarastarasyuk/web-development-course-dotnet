namespace TheaterCashRegister.BLL.Exception;

public class EntityIllegalStateException : System.Exception
{
    public EntityIllegalStateException(string message)
        : base(message)
    {
    }
}