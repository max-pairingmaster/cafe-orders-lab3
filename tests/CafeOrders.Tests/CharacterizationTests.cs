using CafeOrders.Core.Domain;
using Xunit;

namespace CafeOrders.Tests;

public class CharacterizationTests
{
    // Лістинг 1 зі звіту
    [Fact]
    public void Process_RegularClient_Returns1292()
    {
        List<OrderLine> lines = new()
        {
            new OrderLine { Sku = "A1", Quantity = 12, UnitPrice = 100m },
            new OrderLine { Sku = "B2", Quantity = 1, UnitPrice = 250m }
        };

        decimal total = LegacyProcessor.Process(
            1, "Іваненко", "i@ex.com", true, lines,
            new DateOnly(2026, 3, 10), "", "New",
            false, 60m, out string next);

        Assert.Equal(1292.70m, total);
        Assert.Equal("Paid", next);
    }

    [Fact]
    public void Process_SmallOrderWithDelivery_AddsDeliveryFee()
    {
        List<OrderLine> lines = new()
        {
            new OrderLine { Sku = "C3", Quantity = 1, UnitPrice = 200m }
        };

        decimal total = LegacyProcessor.Process(
            2, "Петренко", "p@ex.com", false, lines,
            new DateOnly(2026, 3, 11), "", "New",
            false, 50m, out string next);

        // 200 + 50 (доставка бо < 1000) = 250
        Assert.Equal(250.00m, total);
        Assert.Equal("Paid", next);
    }

    [Fact]
    public void Process_InvalidDocId_ReturnsMinusOne()
    {
        List<OrderLine> lines = new()
        {
            new OrderLine { Sku = "A1", Quantity = 1, UnitPrice = 100m }
        };

        decimal total = LegacyProcessor.Process(
            0, "Тест", "t@ex.com", false, lines,
            new DateOnly(2026, 3, 10), "", "New",
            false, 50m, out string next);

        Assert.Equal(-1m, total);
        Assert.Equal("Error", next);
    }
}
