namespace MetaExchange.Domain.Models;

public class OrderBook
{
    public string Exchange { get; init; } = string.Empty;
    public List<OrderEntry> Bids { get; init; } = [];
    public List<OrderEntry> Asks { get; init; } = [];
}