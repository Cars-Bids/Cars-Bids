using Steria.API.DependencyInjection;
using Steria.API.Hubs;
using Steria.Data.Persistence;
using Steria.API.Extensions;
using Steria.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();


app.UseStaticFiles();
app.UseHttpsRedirection();

//app.UseCors(cfg =>
//{
//    cfg.AllowAnyHeader();
//    cfg.AllowAnyMethod();
//    cfg.AllowAnyOrigin();
//});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine("Request path: " + context.Request.Path);
//     await next.Invoke();
// });
app.MapHub<AuctionHub>("/hub/auction");
app.MapHub<ChatHub>("/hub/chat");
app.MapHub<NotificationHub>("/hub/notifications");

app.MapControllers();

await app.SeedDataAsync();

app.Run();