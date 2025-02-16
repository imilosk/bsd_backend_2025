using MetaExchange.Core.Parsers;
using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Services;

public class OrderBookService : IOrderBookService
{
    private const string OrderBookDataPath = "../../../../../.data/order_books_data.json";

    public List<OrderBook> GetAllOrderBooks()
    {
        return OrderBookParser.Parse(OrderBookDataPath);
    }
}