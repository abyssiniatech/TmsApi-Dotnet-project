using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Generate a short 8-character correlation ID (Exercise 1B requirement)
        string correlationId = Guid.NewGuid().ToString("N")[..8];

        // 2. Attach it to the response headers BEFORE downstream middleware processes it
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        // 3. Begin execution duration tracking
        var stopwatch = Stopwatch.StartNew();

        // 4. Log the Entry Line (tied to the unique correlation ID)
        _logger.LogInformation(
            "[{CorrelationId}] Request Started: {Method} {Path}", 
            correlationId, 
            context.Request.Method, 
            context.Request.Path
        );

        try
        {
            // Pass execution ring context downstream to next middleware/endpoint
            await _next(context);
        }
        finally
        {
            // 5. Halt tracking and log the Exit Line even if downstream code exceptions occur
            stopwatch.Stop();
            
            _logger.LogInformation(
                "[{CorrelationId}] Request Finished: {StatusCode} in {ElapsedMs}ms", 
                correlationId, 
                context.Response.StatusCode, 
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
