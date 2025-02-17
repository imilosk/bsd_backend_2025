using MetaExchange.Core.Parsers;
using MetaExchange.Core.Services;
using MetaExchange.Domain.Enums;
using MetaExchange.Domain.Models;
using MetaExchange.Infrastructure.Data;
using Moq;

namespace MetaExchange.UnitTests;

public class OrderExecutionServiceTests
{
    private const string TestDataPath = "../../../../../.test-data/order_books_data.json";

    private readonly OrderExecutionService _orderExecutionService;

    public OrderExecutionServiceTests()
    {
        Mock<IOrderBookService> mockOrderBookService = new();
        Mock<IExchangeBalanceRepository> mockExchangeBalanceRepository = new();
        _orderExecutionService = new OrderExecutionService(
            mockOrderBookService.Object,
            mockExchangeBalanceRepository.Object
        );

        var orderBooks = LoadOrderBooksFromFile();

        mockOrderBookService
            .Setup(s => s.GetAllOrderBooks())
            .Returns(orderBooks);

        mockExchangeBalanceRepository
            .Setup(s => s.GetBalance(It.IsAny<string>()))
            .Returns(new Balance
            {
                Eur = 10000m,
                Btc = 5m
            });
    }

    private static async Task<List<OrderBook>> LoadOrderBooksFromFile()
    {
        return await OrderBookParser.Parse(TestDataPath);
    }

    [Fact]
    public async Task GetBestExecutionPlan_ShouldReturnCorrectBuyOrders()
    {
        var result = await _orderExecutionService.GetBestExecutionPlan(OrderType.Buy, 1.5m);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(OrderType.Buy, result.First().Type);
        Assert.True(result.Sum(o => o.Amount) >= 1.5m);
    }

    [Fact]
    public async Task GetBestExecutionPlan_ShouldReturnCorrectSellOrders()
    {
        var result = await _orderExecutionService.GetBestExecutionPlan(OrderType.Sell, 3m);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(OrderType.Sell, result.First().Type);
        Assert.True(result.Sum(o => o.Amount) >= 3m);
    }

    [Fact]
    public async Task GetBestExecutionPlan_ShouldThrowExceptionForUnknownOrderType()
    {
        await Assert.ThrowsAsync<Exception>(async () =>
            await _orderExecutionService.GetBestExecutionPlan((OrderType)999, 1.0m)
        );
    }
}