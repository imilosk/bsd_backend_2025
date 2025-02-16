using MetaExchange.Core.Services;
using MetaExchange.Domain.Enums;
using MetaExchange.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchange.Api.Controllers;

[ApiController]
[Route("api/order-execution")]
public class OrderExecutionController : Controller
{
    private readonly IOrderExecutionService _orderExecutionService;

    public OrderExecutionController(IOrderExecutionService orderExecutionService)
    {
        _orderExecutionService = orderExecutionService;
    }

    [HttpGet("best-execution")]
    public ActionResult<List<Order>> GetBestExecution([FromQuery] OrderType orderType, [FromQuery] decimal amount)
    {
        return _orderExecutionService.GetBestExecutionPlan(orderType, amount);
    }
}