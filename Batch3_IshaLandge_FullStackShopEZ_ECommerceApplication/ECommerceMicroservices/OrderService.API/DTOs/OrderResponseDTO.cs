public class OrderResponseDTO
{
    // Unique order identifier
    public int OrderId { get; set; }

    // ID of the user who placed the order
    public int UserId { get; set; }

    // Date and time when the order was placed
    public DateTime OrderDate { get; set; }

    // Total price of the order
    public decimal TotalAmount { get; set; }

    // Current order status
    public string Status { get; set; } = "Order Placed";

    // List of ordered products
    public List<OrderItemResponseDTO> Items { get; set; } = new();
}

public class OrderItemResponseDTO
{
    // Ordered product ID
    public int ProductId { get; set; }

    // Ordered product name
    public string ProductName { get; set; } = string.Empty;

    // Price of a single product unit
    public decimal UnitPrice { get; set; }

    // Quantity ordered
    public int Quantity { get; set; }
}