using MetaExchange.Core.Services;
using MetaExchange.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MetaExchange.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetaExchangeCore(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IOrderBookService, OrderBookService>()
            .AddScoped<IOrderExecutionService, OrderExecutionService>()
            .AddScoped<IExchangeBalanceRepository, ExchangeBalanceRepository>();

        return serviceCollection;
    }
}