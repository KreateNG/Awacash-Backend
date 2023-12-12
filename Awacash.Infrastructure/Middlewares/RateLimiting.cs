using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _requestsPerSecond;
    private readonly object _lock = new object();
    private DateTime _lastRequestTime = DateTime.MinValue;

    public RateLimitMiddleware(RequestDelegate next, int requestsPerSecond)
    {
        _next = next;
        _requestsPerSecond = requestsPerSecond;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        lock (_lock)
        {
            var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
            if (timeSinceLastRequest < TimeSpan.FromSeconds(1))
            {
                context.Response.StatusCode = 429; // 429 Too Many Requests status code
                return;
            }

            _lastRequestTime = DateTime.Now;
        }

        await _next(context);
    }
}
