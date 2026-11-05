namespace CafeOrders.Core.Domain;

public static class DocumentTotalCalculator
{
    public const decimal BulkItemDiscountRate = 0.05m;
    public const decimal RegularClientDiscountRate = 0.07m;
    public const decimal VolumeDiscountRate = 0.10m;
    public const decimal WeekendDiscountRate = 0.03m;
    public const decimal FreeDeliveryThreshold = 1000m;
    public const decimal VolumeDiscountThreshold = 5000m;

    public static decimal SubtotalOf(IReadOnlyList<OrderLine> lines)
    {
        decimal subtotal = 0m;
        foreach (var line in lines)
        {
            if (line.Quantity >= 10)
            {
                subtotal += line.Quantity * line.UnitPrice * (1m - BulkItemDiscountRate);
            }
            else
            {
                subtotal += line.Quantity * line.UnitPrice;
            }
        }
        return subtotal;
    }

    public static decimal ClientDiscountOf(decimal subtotal, bool isRegular)
    {
        return isRegular ? subtotal * RegularClientDiscountRate : 0m;
    }

    public static decimal VolumeDiscountOf(decimal subtotal)
    {
        return subtotal > VolumeDiscountThreshold ? subtotal * VolumeDiscountRate : 0m;
    }

    public static decimal CouponDiscountOf(decimal amount, string coupon)
    {
        if (string.IsNullOrWhiteSpace(coupon))
        {
            return 0m;
        }

        return coupon switch
        {
            "PROMO10" => 100m,
            "CAFE50" => 50m,
            "VIP" => amount * 0.15m,
            _ => 0m
        };
    }

    public static decimal WeekendDiscountOf(decimal amount, DateOnly date)
    {
        bool isWeekend = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend ? amount * WeekendDiscountRate : 0m;
    }

    public static decimal DeliveryFeeOf(decimal subtotal, decimal deliveryFee)
    {
        return subtotal >= FreeDeliveryThreshold ? 0m : deliveryFee;
    }

    public static DocumentStatus NextStatusOf(DocumentStatus currentStatus, decimal total)
    {
        return currentStatus switch
        {
            DocumentStatus.New when total > 0 => DocumentStatus.Paid,
            DocumentStatus.Preparing => DocumentStatus.Ready,
            DocumentStatus.Ready => DocumentStatus.Served,
            _ => currentStatus
        };
    }

    // Диспетчер (єдина відповідальність - координація кроків)
    public static TotalResult Calculate(TotalRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        DocumentGuards.EnsureDocumentIdValid(request.DocumentId);
        DocumentGuards.EnsureClientNameValid(request.Client.Name);
        DocumentGuards.EnsureLinesValid(request.Lines);

        decimal subtotal = SubtotalOf(request.Lines);
        decimal clientDiscount = ClientDiscountOf(subtotal, request.Client.IsRegular);
        decimal afterClient = subtotal - clientDiscount;

        decimal volumeDiscount = VolumeDiscountOf(afterClient);
        decimal afterVolume = afterClient - volumeDiscount;

        decimal couponDiscount = CouponDiscountOf(afterVolume, request.Coupon);
        decimal afterCoupon = afterVolume - couponDiscount;

        decimal weekendDiscount = WeekendDiscountOf(afterCoupon, request.CreatedAt);
        decimal afterWeekend = afterCoupon - weekendDiscount;

        decimal deliveryFee = DeliveryFeeOf(afterWeekend, request.DeliveryFee);
        decimal total = Math.Max(0m, afterWeekend + deliveryFee);
        decimal finalTotal = Math.Round(total, 2);

        DocumentStatus nextStatus = NextStatusOf(request.Status, finalTotal);
        decimal totalDiscount = subtotal - afterWeekend;

        return new TotalResult(finalTotal, nextStatus, subtotal, totalDiscount, deliveryFee);
    }
}
