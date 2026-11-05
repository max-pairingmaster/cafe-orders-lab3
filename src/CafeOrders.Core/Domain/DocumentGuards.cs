namespace CafeOrders.Core.Domain;

public static class DocumentGuards
{
    public static void EnsureDocumentIdValid(int docId)
    {
        if (docId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(docId), "ID документа має бути більшим за 0.");
        }
    }

    public static void EnsureClientNameValid(string? clientName)
    {
        if (string.IsNullOrWhiteSpace(clientName))
        {
            throw new ArgumentException("Ім'я клієнта не може бути порожнім.", nameof(clientName));
        }
    }

    public static void EnsureLinesValid(IReadOnlyList<OrderLine>? lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        if (lines.Count == 0)
        {
            throw new ArgumentException("Замовлення повинно містити хоча б один рядок.", nameof(lines));
        }

        foreach (var line in lines)
        {
            if (line.Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lines), "Кількість товару в рядку має бути більшою за 0.");
            }

            if (line.UnitPrice < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lines), "Ціна товару не може бути від'ємною.");
            }
        }
    }
}
