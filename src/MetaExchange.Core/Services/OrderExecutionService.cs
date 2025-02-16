using MetaExchange.Domain.Enums;
using MetaExchange.Domain.Models;
using MetaExchange.Infrastructure.Data;

namespace MetaExchange.Core.Services;

public class OrderExecutionService : IOrderExecutionService
{
    private readonly IOrderBookService _orderBookService;
    private readonly IExchangeBalanceRepository _exchangeBalanceRepository;

    public OrderExecutionService(
        IOrderBookService orderBookService,
        IExchangeBalanceRepository exchangeBalanceRepository
    )
    {
        _orderBookService = orderBookService;
        _exchangeBalanceRepository = exchangeBalanceRepository;
    }

    public List<Order> GetBestExecutionPlan(OrderType orderType, decimal amount)
    {
        var orderBooks = _orderBookService.GetAllOrderBooks();

        if (orderType == OrderType.Buy)
        {
            var sortedOrders = orderBooks
                .SelectMany(ob => ob.Asks)
                .OrderBy(o => o.Order.Price);

            return ExecuteExecutionPlan(sortedOrders, orderType, amount);
        }

        if (orderType == OrderType.Sell)
        {
            var sortedOrders = orderBooks
                .SelectMany(ob => ob.Bids)
                .OrderByDescending(o => o.Order.Price);

            return ExecuteExecutionPlan(sortedOrders, orderType, amount);
        }

        throw new Exception("Unknown order type");
    }

    private static List<Order> ExecuteExecutionPlan(
        IOrderedEnumerable<OrderEntry> sortedOrders,
        OrderType orderType,
        decimal amount
    )
    {
        var orderPlan = new List<Order>();

        var remaining = amount;
        foreach (var order in sortedOrders)
        {
            if (remaining <= 0)
            {
                break;
            }

            var volume = Math.Min(order.Order.Amount, remaining);
            orderPlan.Add(new Order
            {
                Exchange = order.Order.Exchange,
                Price = order.Order.Price,
                Amount = volume,
                Type = orderType
            });
            remaining -= volume;
        }

        return orderPlan;
    }
}