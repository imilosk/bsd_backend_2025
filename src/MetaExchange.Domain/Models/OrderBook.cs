namespace MetaExchange.Domain.Models;

public class OrderBook
{
    public string Exchange { get; set; } = string.Empty;
    public List<OrderEntry> Bids { get; init; } = [];
    public List<OrderEntry> Asks { get; init; } = [];
}