using CafeOrders.Core.Domain;
using Xunit;

namespace CafeOrders.Tests;

public class ReportTests
{
    [Fact]
    public void TextOf_PureFunction_ReturnsFormattedReceipt()
    {
        var lines = new List<OrderLine>
        {
            new OrderLine { Sku = "A1", Quantity = 1, UnitPrice = 100m }
        };

        var request = new TotalRequest(
            DocumentId: 10,
            Client: new ClientInfo("Іваненко", "i@ex.com", IsRegular: false),
            Lines: lines,
            CreatedAt: new DateOnly(2026, 3, 10),
            Coupon: "",
            Status: DocumentStatus.New,
            DeliveryFee: 50m);

        var result = DocumentTotalCalculator.Calculate(request);
        string text = DocumentReport.TextOf(request, result);

        Assert.Contains("[Receipt] Doc: 10", text);
        Assert.Contains("Client: Іваненко", text);
        Assert.Contains("Status: Paid", text);
    }

    [Fact]
    public void Print_WithCustomTextWriter_WritesReportToWriter()
    {
        var lines = new List<OrderLine>
        {
            new OrderLine { Sku = "B2", Quantity = 2, UnitPrice = 50m }
        };

        var request = new TotalRequest(
            DocumentId: 11,
            Client: new ClientInfo("Петренко", "p@ex.com", IsRegular: true),
            Lines: lines,
            CreatedAt: new DateOnly(2026, 3, 11),
            Coupon: "",
            Status: DocumentStatus.New,
            DeliveryFee: 0m);

        var result = DocumentTotalCalculator.Calculate(request);
        string text = DocumentReport.TextOf(request, result);

        using var writer = new StringWriter();
        DocumentReportPrinter.Print(writer, text);

        string writtenOutput = writer.ToString();
        Assert.Equal(text, writtenOutput);
    }
}
