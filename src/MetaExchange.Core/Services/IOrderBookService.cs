using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Services;

public interface IOrderBookService
{
    List<OrderBook> GetAllOrderBooks();
}