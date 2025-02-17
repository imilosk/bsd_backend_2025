using MetaExchange.Core.Extensions;
using MetaExchange.Core.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddJsonFile("secrets.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services
    .AddOpenApi()
    .AddMetaExchangeCore(configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Preload order books on startup
var orderBookService = app.Services.GetRequiredService<IOrderBookService>();
await ((OrderBookService)orderBookService).GetAllOrderBooks();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();