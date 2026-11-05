namespace CafeOrders.Core.Domain;

public record TotalResult(
    decimal Total,
    DocumentStatus NextStatus,
    decimal Subtotal,
    decimal Discount,
    decimal DeliveryFee);
