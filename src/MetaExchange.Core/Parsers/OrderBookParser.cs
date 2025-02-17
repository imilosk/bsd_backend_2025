using System.Buffers;
using System.IO.Pipelines;
using System.Text.Json;
using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Parsers;

public static class OrderBookParser
{
    public static async Task<List<OrderBook>> Parse(string path)
    {
        var fileStream = File.OpenRead(path);
        var pipeReader = PipeReader.Create(fileStream);

        var allOrderBooks = new List<OrderBook>();
        var exchangeCounter = 1;

        while (true)
        {
            var result = await pipeReader.ReadAsync();
            var buffer = result.Buffer;

            while (TryReadLine(ref buffer, out var line))
            {
                var exchangeName = $"exchange-{exchangeCounter}";

                var orderBook = ProcessOrderBook(line, exchangeName);
                allOrderBooks.Add(orderBook);

                exchangeCounter++;
            }

            pipeReader.AdvanceTo(buffer.Start, buffer.End);

            if (result.IsCompleted)
            {
                break;
            }
        }

        return allOrderBooks;
    }

    private static OrderBook ProcessOrderBook(ReadOnlySequence<byte> sequence, string exchangeName)
    {
        var reader = new SequenceReader<byte>(sequence);

        if (!reader.TryReadTo(out ReadOnlySequence<byte> _, (byte)'\t', advancePastDelimiter: true))
        {
            throw new FormatException("No tab character found");
        }

        var jsonReader = new Utf8JsonReader(reader.UnreadSequence);
        var orderBook = JsonSerializer.Deserialize<OrderBook>(ref jsonReader) ??
                        throw new JsonException("Invalid order book");

        orderBook.Exchange = exchangeName;

        orderBook.Asks.ForEach(x => x.Order.Exchange = exchangeName);
        orderBook.Bids.ForEach(x => x.Order.Exchange = exchangeName);

        return orderBook;
    }

    private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
    {
        var reader = new SequenceReader<byte>(buffer);
        if (reader.TryReadTo(out line, (byte)'\n', advancePastDelimiter: true))
        {
            buffer = buffer.Slice(reader.Position);

            return true;
        }

        line = default;

        return false;
    }
}