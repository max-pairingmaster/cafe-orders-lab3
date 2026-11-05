using CafeOrders.Core.Domain;
using Xunit;

namespace CafeOrders.Tests;

public class CalculatorTests
{
    [Fact]
    public void Calculate_RegularClient_Returns1292()
    {
        var lines = new List<OrderLine>
        {
            new OrderLine { Sku = "A1", Quantity = 12, UnitPrice = 100m },
            new OrderLine { Sku = "B2", Quantity = 1, UnitPrice = 250m }
        };

        var request = new TotalRequest(
            DocumentId: 1,
            Client: new ClientInfo("Іваненко", "i@ex.com", IsRegular: true),
            Lines: lines,
            CreatedAt: new DateOnly(2026, 3, 10),
            Coupon: "",
            Status: DocumentStatus.New,
            DeliveryFee: 60m);

        var result = DocumentTotalCalculator.Calculate(request);

        Assert.Equal(1292.70m, result.Total);
        Assert.Equal(DocumentStatus.Paid, result.NextStatus);
    }

    [Fact]
    public void SubtotalOf_BulkQuantity_AppliesFivePercentDiscount()
    {
        var lines = new List<OrderLine>
        {
            new OrderLine { Sku = "A1", Quantity = 10, UnitPrice = 100m }
        };

        decimal subtotal = DocumentTotalCalculator.SubtotalOf(lines);
        // 10 * 100 * 0.95 = 950
        Assert.Equal(950m, subtotal);
    }

    [Fact]
    public void DeliveryFeeOf_SubtotalOver1000_ReturnsZero()
    {
        decimal fee = DocumentTotalCalculator.DeliveryFeeOf(1200m, 60m);
        Assert.Equal(0m, fee);
    }
}
