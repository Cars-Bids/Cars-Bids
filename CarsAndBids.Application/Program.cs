using CarsAndBids.API.Middleware;
using CarsAndBids.API.DependencyInjection;
using CarsAndBids.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();


app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors(cfg =>
{
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
    cfg.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine("Request path: " + context.Request.Path);
//     await next.Invoke();
// });

app.MapHub<ChatHub>("/hub/chat");

app.MapControllers();

app.Run();