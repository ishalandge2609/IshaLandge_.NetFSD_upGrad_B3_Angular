

namespace OrderService.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}