namespace CafeOrders.Core.Domain;

public static class DocumentReportPrinter
{
    // Команда з побічним ефектом (пише в потік TextWriter)
    public static void Print(TextWriter writer, string reportText)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(reportText);

        writer.Write(reportText);
    }
}
