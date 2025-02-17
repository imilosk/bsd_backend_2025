using System.Globalization;
using MetaExchange.Core.Extensions;
using MetaExchange.Core.Services;
using MetaExchange.Domain.Enums;
using MetaExchange.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Force decimal point
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

var services = new ServiceCollection();

services.AddMetaExchangeCore(configuration);

var serviceProvider = services.BuildServiceProvider();

var orderExecutionService = serviceProvider.GetRequiredService<IOrderExecutionService>();

if (!TryParseArgs(args, out var parsedArgs))
{
    return 1;
}

var bestExecutionPlan = await orderExecutionService.GetBestExecutionPlan(parsedArgs.orderType, parsedArgs.amount);
PrintExecutionPlan(bestExecutionPlan);

return 0;


static void PrintExecutionPlan(List<Order> executionPlans)
{
    Console.WriteLine("Best execution plan: ");
    foreach (var order in executionPlans)
    {
        Console.WriteLine(
            $"Exchange: {order.Exchange}, amount: {order.Amount}, price: {order.Price}"
        );
    }
}

static bool TryParseArgs(string[] args, out (OrderType orderType, decimal amount) parsedArgs)
{
    parsedArgs = (OrderType.None, -1);

    if (args.Length < 2)
    {
        Console.WriteLine("Usage: MetaExchange.ConsoleApp <OrderType> <Amount>");
        Console.WriteLine("Example: MetaExchange.ConsoleApp Buy 10.05");

        return false;
    }

    var orderTypeString = args[0];
    if (!Enum.TryParse<OrderType>(orderTypeString, true, out var orderType))
    {
        Console.WriteLine($"Invalid order type '{orderTypeString}'. Accepted values: Buy / Sell");

        return false;
    }

    var amountString = args[1];
    if (!decimal.TryParse(amountString, out var amount))
    {
        Console.WriteLine($"Invalid amount '{amountString}'. Provide a numeric value (e.g. 10.05)");
        return false;
    }

    parsedArgs = (orderType, amount);

    return true;
}