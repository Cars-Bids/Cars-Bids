using CarsAndBids.API.DependencyInjection;
using CarsAndBids.API.Hubs;
using CarsAndBids.Data.Persistence;
using CarsAndBids.API.Extensions;
using CarsAndBids.API.Middleware;
using CarsAndBids.Core.Hubs;

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

app.MapHub<AuctionHub>("/auctionHub").RequireAuthorization();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine("Request path: " + context.Request.Path);
//     await next.Invoke();
// });

app.MapHub<ChatHub>("/hub/chat");

app.MapControllers();

await app.SeedDataAsync();

app.Run();