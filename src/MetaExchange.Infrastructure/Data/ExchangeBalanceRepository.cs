using MetaExchange.Domain.Models;

namespace MetaExchange.Infrastructure.Data;

public class ExchangeBalanceRepository : IExchangeBalanceRepository
{
    private const int RandomSeed = 42;
    private readonly Random _random = new(RandomSeed);

    private readonly Dictionary<string, Balance> _balances = new();

    public Balance GetBalance(string exchange)
    {
        var exists = _balances.TryGetValue(exchange, out var balance);

        if (exists && balance is not null)
        {
            return balance;
        }

        var eurBalance = Math.Round((decimal)(_random.NextDouble() * 100000), 2);
        var btcBalance = Math.Round((decimal)(_random.NextDouble() * 50), 5);

        _balances[exchange] = new Balance
        {
            Btc = btcBalance,
            Eur = eurBalance
        };

        balance = _balances[exchange];

        return balance;
    }
}