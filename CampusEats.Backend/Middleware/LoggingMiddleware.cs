namespace CampusEats.Middleware;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
        await next(context);
        logger.LogInformation("Finished handling request: {StatusCode}", context.Response.StatusCode);
    }
}