using CarsAndBids.API.DependencyInjection;
using CarsAndBids.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors(cfg =>
{
    cfg.AllowAnyHeader();
    cfg.AllowAnyMethod();
    cfg.AllowAnyOrigin();
});

app.UseAuthorization();

// app.Use(async (context, next) =>
// {
//     Console.WriteLine("Request path: " + context.Request.Path);
//     await next.Invoke();
// });

app.MapControllers();

app.Run();