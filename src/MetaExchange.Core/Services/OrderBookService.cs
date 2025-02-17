using MetaExchange.Core.Parsers;
using MetaExchange.Core.Settings;
using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Services;

public class OrderBookService : IOrderBookService
{
    private readonly OrderBookSettings _orderBookSettings;
    private static List<OrderBook>? _orderBooksCache;

    public OrderBookService(OrderBookSettings orderBookSettings)
    {
        _orderBookSettings = orderBookSettings;
    }

    public async Task<List<OrderBook>> GetAllOrderBooks()
    {
        if (_orderBooksCache is not null)
        {
            return _orderBooksCache;
        }

        var orderBooks = await OrderBookParser.Parse(_orderBookSettings.OrderBooksPath);
        _orderBooksCache = orderBooks;

        return orderBooks;
    }
}