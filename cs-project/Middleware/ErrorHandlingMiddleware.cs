using System.Net;
using System.Text.Json;

namespace cs_project.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problemDetails = new
                {
                    status = context.Response.StatusCode,
                    title = "An unexpected error occurred.",
                    detail = ex.Message
#if DEBUG
                    ,
                    stackTrace = ex.StackTrace
#endif
                };
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
            }
        }
    }
}
