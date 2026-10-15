using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace FoodOpsERP.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // ✅ Log with context
            _logger.LogError(
                exception,
                "Unhandled exception. Path: {Path}, Method: {Method}, User: {User}",
                context.Request.Path,
                context.Request.Method,
                context.User?.Identity?.Name ?? "Anonymous");

            // ✅ AJAX detection
            bool isAjaxRequest =
                context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                context.Request.Headers["Accept"].ToString().Contains("application/json") ||
                context.Request.Path.StartsWithSegments("/api");

            // ✅ Status code based on exception type
            var (statusCode, errorMessage) = MapException(exception, _env.IsDevelopment());

            // ✅ If response already started, can't write — just log and return
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response already started. Cannot write error response.");
                return;
            }

            context.Response.StatusCode = statusCode;

            if (isAjaxRequest)
            {
                // ✅ AJAX / SPA → JSON response
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    statusCode = statusCode,
                    message = errorMessage,
                    detail = _env.IsDevelopment() ? exception.StackTrace : null
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            else
            {
                // ✅ Normal request → safe HTML
                context.Response.ContentType = "text/html";

                var safeMessage = HtmlEncoder.Default.Encode(errorMessage);

                var html = $@"
                    <div class='alert alert-danger m-4'>
                        <h5>Something went wrong</h5>
                        <p>{safeMessage}</p>
                    </div>";

                await context.Response.WriteAsync(html);
            }
        }

        // ✅ Map exception type to status code + message
        private (int statusCode, string message) MapException(Exception exception, bool isDev)
        {
            return exception switch
            {
                ArgumentNullException => (StatusCodes.Status400BadRequest, exception.Message),
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                InvalidOperationException => (StatusCodes.Status400BadRequest, exception.Message),

                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),

                _ => (StatusCodes.Status500InternalServerError,
                      isDev ? exception.Message : "An unexpected error occurred. Please try again later.")
            };
        }
    }
}