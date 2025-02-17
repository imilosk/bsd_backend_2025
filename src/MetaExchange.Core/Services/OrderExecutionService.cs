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

    public async Task<List<Order>> GetBestExecutionPlan(OrderType orderType, decimal amount)
    {
        var orderBooks = await _orderBookService.GetAllOrderBooks();

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

    private List<Order> ExecuteExecutionPlan(
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

            var balance = _exchangeBalanceRepository.GetBalance(order.Order.Exchange);

            var maxVolume = order.Order.Amount;

            switch (orderType)
            {
                case OrderType.Buy:
                {
                    var affordableVolume = balance.Eur / order.Order.Price;
                    maxVolume = Math.Min(maxVolume, affordableVolume);
                    break;
                }
                case OrderType.Sell:
                    maxVolume = Math.Min(maxVolume, balance.Btc);
                    break;
            }

            if (maxVolume <= 0)
            {
                continue;
            }

            var volumeToFill = Math.Min(maxVolume, remaining);

            if (orderType == OrderType.Buy)
            {
                balance.Eur -= volumeToFill * order.Order.Price;
            }
            else
            {
                balance.Btc -= volumeToFill;
            }

            orderPlan.Add(new Order
            {
                Exchange = order.Order.Exchange,
                Price = order.Order.Price,
                Amount = volumeToFill,
                Type = orderType
            });

            remaining -= volumeToFill;
        }

        return orderPlan;
    }
}