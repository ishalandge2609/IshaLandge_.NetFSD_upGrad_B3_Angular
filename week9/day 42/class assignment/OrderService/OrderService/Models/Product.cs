namespace OrderService.Models
{

    //In microservices, services should remain independent.
    //Order Service creates its own Product model to consume Product Service API response without directly depending on Product Service code.
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
