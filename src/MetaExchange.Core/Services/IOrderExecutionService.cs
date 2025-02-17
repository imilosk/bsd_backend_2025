using MetaExchange.Domain.Enums;
using MetaExchange.Domain.Models;

namespace MetaExchange.Core.Services;

public interface IOrderExecutionService
{
    Task<List<Order>> GetBestExecutionPlan(OrderType orderType, decimal amount);
}