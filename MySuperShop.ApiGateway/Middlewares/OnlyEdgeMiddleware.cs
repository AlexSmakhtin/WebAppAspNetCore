namespace MySuperShop.ApiGateway.Middleware;

public class OnlyEdgeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OnlyEdgeMiddleware> _logger;

    public OnlyEdgeMiddleware(RequestDelegate next, ILogger<OnlyEdgeMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userAgent = context.Request.Headers.UserAgent.ToString();
        if (userAgent.Contains("Edg"))
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Edge Browser required.");
        }
    }
}