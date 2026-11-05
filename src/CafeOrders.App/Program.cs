using System.Text;
using CafeOrders.Core.Domain;

namespace CafeOrders.App;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("========================================");
        Console.WriteLine("   СИСТЕМА РОЗРАХУНКУ ЗАМОВЛЕНЬ КАФЕ    ");
        Console.WriteLine("   Лабораторна робота № 3 (Варіант 12)  ");
        Console.WriteLine("========================================");

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

        Console.WriteLine("Виконуємо розрахунок замовлення (DocumentTotalCalculator.Calculate)...");
        var result = DocumentTotalCalculator.Calculate(request);

        Console.WriteLine($"Підсумок: {result.Total:F2} грн (Знижка: {result.Discount:F2} грн)");
        Console.WriteLine($"Новий статус: {result.NextStatus}");
        Console.WriteLine("\n--- ТЕКСТ ЗВІТУ (CQS: DocumentReport.TextOf) ---");

        string reportText = DocumentReport.TextOf(request, result);
        DocumentReportPrinter.Print(Console.Out, reportText);

        Console.WriteLine("========================================");
    }
}
