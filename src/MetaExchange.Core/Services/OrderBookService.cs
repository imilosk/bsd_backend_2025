using MetaExchange.Core.Parsers;
using MetaExchange.Core.Settings;
using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Services;

public class OrderBookService : IOrderBookService
{
    private readonly OrderBookSettings _orderBookSettings;

    public OrderBookService(OrderBookSettings orderBookSettings)
    {
        _orderBookSettings = orderBookSettings;
    }

    public List<OrderBook> GetAllOrderBooks()
    {
        return OrderBookParser.Parse(_orderBookSettings.OrderBooksPath);
    }
}