namespace CafeOrders.Core.Domain;

public record TotalRequest(
    int DocumentId,
    ClientInfo Client,
    IReadOnlyList<OrderLine> Lines,
    DateOnly CreatedAt,
    string Coupon,
    DocumentStatus Status,
    decimal DeliveryFee);
