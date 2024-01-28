using System.Net;

namespace MySuperShop.Domain.Exceptions;

public class MySuperShopException : Exception
{
    public HttpStatusCode? StatusCode { get; }

    public MySuperShopException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public MySuperShopException(string message) : base(message)
    {
    }
}