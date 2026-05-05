using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Exceptions;
using ShopEZ.WebAPI.Models;
using ShopEZ.WebAPI.Repositories;
using ShopEZ.WebAPI.Exceptions;

namespace ShopEZ.WebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        //  CREATE ORDER
        public async Task<OrderResponseDTO> CreateOrderAsync(OrderCreateDTO dto)
        {
            // Create Order object
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            // Loop through items
            foreach (var item in dto.Items)
            {
                //  1. Fetch product
                var product = await _productRepo.GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new NotFoundException($"Product with ID {item.ProductId} not found");

                // 2. Validate quantity
                if (item.Quantity <= 0)
                    throw new BadRequestException("Quantity must be greater than 0");

                // 3. Check stock
                if (product.Stock < item.Quantity)
                    throw new BadRequestException(
                        $"Only {product.Stock} items available for {product.Name}"
                    );

                // 4. Deduct stock
                product.Stock -= item.Quantity;

                //  IMPORTANT: Save updated stock
                await _productRepo.UpdateAsync(product);

                //  5. Create order item
                var orderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                order.OrderItems.Add(orderItem);

                // 6. Calculate total
                totalAmount += product.Price * item.Quantity;
            }

            // 7. Set total amount
            order.TotalAmount = totalAmount;

            // 8. Save order
            await _orderRepo.AddAsync(order);

            // 9. Return response
            return new OrderResponseDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDetailsDTO
                {
                    ProductName = oi.Product?.Name ?? "N/A",
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }


        // GET ALL
        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync();

            return orders.Select(o => new OrderResponseDTO
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemDetailsDTO
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });
        }

        // GET BY ID
        public async Task<OrderResponseDTO> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
                return null;

            return new OrderResponseDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDetailsDTO
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }
    }
}
