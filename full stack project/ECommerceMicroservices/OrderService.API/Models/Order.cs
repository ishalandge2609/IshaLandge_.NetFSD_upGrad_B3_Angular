namespace OrderService.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Status { get; set; } = "Order Placed";

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}