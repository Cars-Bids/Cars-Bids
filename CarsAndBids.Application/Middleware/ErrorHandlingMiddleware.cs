using System.Text.Json;

namespace CarsAndBids.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "API/json";
                var error = new { Message = "An error occurred.", Detail = ex.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(error));
            }
        }
    }
}
