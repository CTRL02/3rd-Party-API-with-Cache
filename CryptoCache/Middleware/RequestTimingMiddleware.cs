using System.Diagnostics;

namespace CryptoCache.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();  // Start stopwatch to measure time
            stopwatch.Start();

            // Continue with the request pipeline
            await _next(context);

            stopwatch.Stop();  // Stop stopwatch after request is handled
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation($"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms.");
        }
    }

}
