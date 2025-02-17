using MetaExchange.Core.Services;
using MetaExchange.Domain.Enums;
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
    public async Task<IActionResult> GetBestExecution([FromQuery] OrderType orderType, [FromQuery] decimal amount)
    {
        var result = await _orderExecutionService.GetBestExecutionPlan(orderType, amount);

        return Ok(result);
    }
}