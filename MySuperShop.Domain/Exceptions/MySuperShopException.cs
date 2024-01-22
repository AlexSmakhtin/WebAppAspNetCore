using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MySuperShop.Domain.Exceptions;

public class MySuperShopException : Exception
{
    public HttpStatusCode? StatusCode { get; }
    public ValidationProblemDetails? Details { get; }
    public MySuperShopException(HttpStatusCode statusCode, ValidationProblemDetails details)
        : base(message: details.Title)
    {
        StatusCode = statusCode;
        Details = details;
    }
    public MySuperShopException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public MySuperShopException(string message) : base(message)
    {
    }
}