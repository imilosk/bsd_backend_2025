using System.Text.Json;
using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Parsers;

public static class OrderBookParser
{
    public static List<OrderBook> Parse(string path)
    {
        var reader = new StreamReader(path);
        var allOrderBooks = new List<OrderBook>();
        var exchangeCounter = 1;

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? string.Empty;
            var parts = line.Split();

            var orderBook = JsonSerializer.Deserialize<OrderBook>(parts[1]) ??
                            throw new JsonException("Invalid order book");

            var exchangeName = $"exchange-{exchangeCounter++}";
            orderBook.Exchange = exchangeName;

            orderBook.Asks.ForEach(x => x.Order.Exchange = exchangeName);
            orderBook.Bids.ForEach(x => x.Order.Exchange = exchangeName);

            allOrderBooks.Add(orderBook);
        }

        return allOrderBooks;
    }
}