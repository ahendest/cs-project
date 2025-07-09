using cs_project.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace cs_project.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            var correlationId = context.TraceIdentifier;
            context.Response.Headers["X-Correlation-ID"] = correlationId;
            _logger.LogError(ex, "Unhandled exception occurred while processing request to {Path}. Correlation ID: {CorrelationId}", context.Request.Path, correlationId);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            object problemDetails = _env.IsDevelopment()
                ? new
                {
                    status = (int)statusCode,
                    title = ex.GetType().Name,
                    detail = ex.Message,
                    stackTrace = ex.StackTrace,
                    correlationId
                }
                : new
                {
                    status = (int)statusCode,
                    title = ex.GetType().Name,
                    detail = statusCode == HttpStatusCode.NotFound ? ex.Message : "An unexpected error occurred.",
                    correlationId
                };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
        }
    }
}