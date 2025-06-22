using System.Net;
using CarsAndBids.Core.Exceptions;

namespace CarsAndBids.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        catch (HttpException ex)
        {
            SendResponse(context, ex.Message, ex.StatusCode);
        }
        catch (Exception ex)
        {
            SendResponse(context, ex.Message);
        }
    }

    private async void SendResponse(HttpContext context, string msg, HttpStatusCode code = HttpStatusCode.InternalServerError)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsJsonAsync(new
        {
            Status = code,
            Message = msg
        });
    }
}