namespace MetaExchange.Domain.Models;

public class Exchange
{
    public string Name { get; set; } = string.Empty;
    public required OrderBook OrderBook { get; set; }
    public decimal EurBalance { get; set; }
    public decimal BtcBalance { get; set; }
}