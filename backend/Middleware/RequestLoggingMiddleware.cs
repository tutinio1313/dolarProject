using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace backend.Middleware;

public class RequestLoggingMiddleware(RequestDelegate requestDelegate, ILoggerFactory logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var httpContextLogger = logger.CreateLogger<RequestLoggingMiddleware>();
        httpContextLogger.LogInformation($"[Time:{DateTime.UtcNow}] - [URL:{context.Request.Host}{context.Request.Path}] - [Method:{context.Request.Method}] - [IP - {context.Connection.RemoteIpAddress}] - [ID - {context.TraceIdentifier}]");
        await requestDelegate(context);
    }        
}