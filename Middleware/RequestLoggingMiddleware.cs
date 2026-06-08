using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // BEFORE NEXT

        var path = context.Request.Path;
        var method = context.Request.Method;

        var correlationId = Guid.NewGuid().ToString();

        context.Response.Headers["X-Correlation-Id"] = correlationId;

        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Request Started: {Method} {Path}",
            method,
            path);

        // Pass control to next middleware
        await _next(context);

        // AFTER NEXT

        stopwatch.Stop();

        var statusCode = context.Response.StatusCode;
        var elapsedMs = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation(
            "Request Completed: {Method} {Path} => {StatusCode} in {ElapsedMs}ms",
            method,
            path,
            statusCode,
            elapsedMs);
    }
}






