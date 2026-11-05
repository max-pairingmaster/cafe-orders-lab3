namespace CafeOrders.Core.Domain;

public enum DocumentStatus
{
    New = 0,
    Preparing = 1,
    Ready = 2,
    Served = 3,
    Paid = 4,
    Cancelled = 5
}
