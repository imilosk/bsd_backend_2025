using MetaExchange.Domain.Enums;

namespace MetaExchange.Domain.Models;

public class OrderEntry
{
    public required Order Order { get; set; }
}

public class Order
{
    public string Exchange { get; set; } = string.Empty;
    public OrderType Type { get; set; }
    public OrderKind Kind { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}