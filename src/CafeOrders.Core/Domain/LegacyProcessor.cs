using System.Text;

namespace CafeOrders.Core.Domain;

public static class LegacyProcessor
{
    // Метод-монстр до рефакторингу (92 рядки, 11 параметрів, ЦС = 31)
    public static decimal Process(
        int docId,
        string clientName,
        string clientEmail,
        bool isRegular,
        List<OrderLine> lines,
        DateOnly createdAt,
        string coupon,
        string status,
        bool printToConsole,
        decimal deliveryFee,
        out string next)
    {
        if (docId <= 0)
        {
            next = "Error";
            return -1m;
        }

        if (string.IsNullOrWhiteSpace(clientName))
        {
            next = "Error";
            return -1m;
        }

        if (lines == null || lines.Count == 0)
        {
            next = "Error";
            return -1m;
        }

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Quantity <= 0 || lines[i].UnitPrice < 0)
            {
                next = "Error";
                return -1m;
            }
        }

        decimal subtotal = 0m;
        for (int i = 0; i < lines.Count; i++)
        {
            var l = lines[i];
            if (l.Quantity >= 10)
            {
                subtotal += l.Quantity * l.UnitPrice * 0.95m;
            }
            else
            {
                subtotal += l.Quantity * l.UnitPrice;
            }
        }

        if (isRegular)
        {
            subtotal *= 0.93m;
        }

        if (subtotal > 5000m)
        {
            subtotal *= 0.90m;
        }

        if (!string.IsNullOrEmpty(coupon))
        {
            if (coupon == "PROMO10")
            {
                subtotal -= 100m;
            }
            else if (coupon == "CAFE50")
            {
                subtotal -= 50m;
            }
            else if (coupon == "VIP")
            {
                subtotal *= 0.85m;
            }
        }

        if (createdAt.DayOfWeek == DayOfWeek.Saturday || createdAt.DayOfWeek == DayOfWeek.Sunday)
        {
            subtotal *= 0.97m;
        }

        if (subtotal < 1000m)
        {
            subtotal += deliveryFee;
        }

        if (subtotal < 0)
        {
            subtotal = 0m;
        }

        if (status == "New" && subtotal > 0)
        {
            next = "Paid";
        }
        else if (status == "Preparing")
        {
            next = "Ready";
        }
        else if (status == "Ready")
        {
            next = "Served";
        }
        else if (status == "Served")
        {
            next = "Completed";
        }
        else
        {
            next = status;
        }

        if (printToConsole)
        {
            // Вилучено пряме логування в термінал з доменної логіки
        }

        return Math.Round(subtotal, 2);
    }
}
