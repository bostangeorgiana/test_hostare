namespace CampusEats.Features.Orders;

public record OrderSummaryDto(
    int OrderId,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt,
    string? Notes
);

public record OrderItemDetailsDto(
    int MenuItemId,
    string MenuItemName,
    int Quantity,
    decimal Price
);

public record OrderDetailsDto(
    int OrderId,
    decimal TotalAmount,
    string Status,
    DateTime CreatedAt,
    List<OrderItemDetailsDto> Items,
    int LoyaltyPointsUsed,
    int TotalLoyaltyPointsEarned,
    decimal FinalAmountPaid
);