using System.Text;

namespace CafeOrders.Core.Domain;

public static class DocumentReport
{
    // Чиста функція (Запит за CQS)
    public static string TextOf(TotalRequest request, TotalResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        var sb = new StringBuilder();
        sb.AppendLine($"[Receipt] Doc: {request.DocumentId}, Client: {request.Client.Name}, Email: {request.Client.Email}");
        sb.AppendLine($"Date: {request.CreatedAt}, Total: {result.Total:F2}, Status: {result.NextStatus}");
        return sb.ToString();
    }
}
