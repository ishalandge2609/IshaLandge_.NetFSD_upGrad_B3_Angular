using OrderService.API.DTOs;
using OrderService.API.Exceptions;
using OrderService.API.Models;
using OrderService.API.Repositories;

namespace OrderService.API.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;

        private readonly ProductServiceClient _productClient;

        private readonly ILogger<OrdersService> _logger;

        public OrdersService(
            IOrderRepository orderRepo,
            ProductServiceClient productClient,
            ILogger<OrdersService> logger)
        {
            _orderRepo = orderRepo;

            _productClient = productClient;

            _logger = logger;
        }

        public async Task<OrderResponseDTO>
            CreateOrderAsync(
            int userId,
            CreateOrderDTO dto,
            string token)
        {
            decimal total = 0;

            var items = new List<OrderItem>();

            foreach (var item in dto.Items)
            {
                var product = await _productClient
                    .GetProductAsync(item.ProductId, token);

                if (product == null)
                {
                    throw new ValidationException(
                        $"Product {item.ProductId} not found");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new ValidationException(
                        $"Insufficient stock for {product.Name}");
                }

                total += product.Price * item.Quantity;

                items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = item.Quantity
                });

                await _productClient.DeductStockAsync(
                    product.Id,
                    item.Quantity,
                    token);
            }

            var order = new Order
            {
                UserId = userId,
                TotalAmount = total,
                Status = "Order Placed",
                OrderItems = items
            };

            await _orderRepo.AddAsync(order);

            return new OrderResponseDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = items.Select(i => new OrderItemResponseDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity

                }).ToList()
            };
        }

        public async Task CancelOrderAsync(
            int orderId,
            int userId,
            bool isAdmin,
            string token)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            if (!isAdmin && order.UserId != userId)
            {
                throw new UnauthorizedException(
                    "You cannot cancel this order");
            }

            if (order.Status == "Cancelled")
            {
                throw new ValidationException(
                    "Order is already cancelled");
            }

            foreach (var item in order.OrderItems)
            {
                await _productClient.RestockProductAsync(
                    item.ProductId,
                    item.Quantity,
                    token);
            }

            order.Status = "Cancelled";

            await _orderRepo.CancelOrderAsync(order);
        }

        public async Task<OrderResponseDTO?>
            GetByIdAsync(int orderId)
        {
            var o = await _orderRepo.GetByIdAsync(orderId);

            if (o == null)
            {
                return null;
            }

            return new OrderResponseDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Items = o.OrderItems.Select(i =>
                    new OrderItemResponseDTO
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity

                    }).ToList()
            };
        }

        public async Task DeleteAsync(
            int orderId,
            int userId,
            bool isAdmin)
        {
            var order = await _orderRepo
                .GetByIdAsync(orderId)
                ?? throw new NotFoundException(
                    "Order not found");

            if (!isAdmin && order.UserId != userId)
            {
                throw new UnauthorizedException(
                    "Not allowed");
            }

            await _orderRepo.SoftDeleteAsync(orderId);
        }

        public async Task<IEnumerable<OrderResponseDTO>>
            GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepo
                .GetOrdersByUserIdAsync(userId);

            return orders.Select(o =>
                new OrderResponseDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Items = o.OrderItems.Select(i =>
                        new OrderItemResponseDTO
                        {
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                            UnitPrice = i.UnitPrice,
                            Quantity = i.Quantity

                        }).ToList()

                }).ToList();
        }

        public async Task<IEnumerable<OrderResponseDTO>>
            GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();

            return orders.Select(o =>
                new OrderResponseDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Items = o.OrderItems.Select(i =>
                        new OrderItemResponseDTO
                        {
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                            UnitPrice = i.UnitPrice,
                            Quantity = i.Quantity

                        }).ToList()

                }).ToList();
        }
    }
}