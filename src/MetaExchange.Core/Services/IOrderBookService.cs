using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Services;

public interface IOrderBookService
{
    Task<List<OrderBook>> GetAllOrderBooks();
}