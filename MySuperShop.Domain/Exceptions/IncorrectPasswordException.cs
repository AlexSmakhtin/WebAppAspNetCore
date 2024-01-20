namespace MySuperShop.Domain.Exceptions;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string message) : base(message)
    {
    }
}