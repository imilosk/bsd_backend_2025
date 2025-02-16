using MetaExchange.Core.Services;
using MetaExchange.Core.Settings;
using MetaExchange.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetaExchange.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMetaExchangeCore(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
    )
    {
        serviceCollection
            .AddAndValidateServiceOptions<OrderBookSettings>(configuration)
            .AddScoped<IOrderBookService, OrderBookService>()
            .AddScoped<IOrderExecutionService, OrderExecutionService>()
            .AddScoped<IExchangeBalanceRepository, ExchangeBalanceRepository>();

        return serviceCollection;
    }

    public static IServiceCollection AddAndValidateServiceOptions<T>(
        this IServiceCollection services,
        IConfiguration configurationRoot
    ) where T : class
    {
        var sectionName = typeof(T).Name;
        var section = configurationRoot.GetSection(sectionName);

        var configValue = section.Get<T>();
        if (configValue is null)
        {
            throw new InvalidOperationException(
                $"Configuration section '{sectionName}' is required for {typeof(T).Name}.");
        }

        services.AddSingleton(configValue);

        services.AddOptions<T>()
            .Configure(options => section
                .Bind(options))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}