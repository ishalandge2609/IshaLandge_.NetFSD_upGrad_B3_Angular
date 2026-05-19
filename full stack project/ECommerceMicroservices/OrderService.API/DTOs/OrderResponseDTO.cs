public class OrderResponseDTO
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Order Placed";

    public List<OrderItemResponseDTO> Items { get; set; } = new();
}

public class OrderItemResponseDTO
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}