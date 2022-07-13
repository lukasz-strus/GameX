namespace gamexAPI.Excepitons;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}