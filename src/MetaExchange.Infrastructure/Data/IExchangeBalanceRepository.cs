using MetaExchange.Domain.Models;

namespace MetaExchange.Infrastructure.Data;

public interface IExchangeBalanceRepository
{
    Balance GetBalance(string exchange);
}