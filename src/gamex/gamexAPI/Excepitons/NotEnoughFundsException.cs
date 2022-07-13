namespace gamexAPI.Excepitons;

public class NotEnoughFundsException : Exception
{
    public NotEnoughFundsException(string message) : base(message)
    {
    }
}