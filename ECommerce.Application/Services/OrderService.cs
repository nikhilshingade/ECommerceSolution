using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

public class OrderService : IOrderService
{
    private readonly ICartRepository _cartRepo;
    private readonly IOrderRepository _orderRepo;

    public OrderService(
        ICartRepository cartRepo,
        IOrderRepository orderRepo)
    {
        _cartRepo = cartRepo;
        _orderRepo = orderRepo;
    }

    public async Task PlaceOrderAsync(int userId)
    {
        // 1. Get cart
        var cart = await _cartRepo.GetUserCartAsync(userId);

        if (cart == null || !cart.CartItems.Any())
            throw new Exception("Cart is empty");

        // 2. Create order
        var order = new Order
        {
            UserId = userId,
            TotalAmount = cart.CartItems.Sum(ci =>
                ci.Product.Price * ci.Quantity),
            Status = "Pending"
        };

        // 3. Create order items
        foreach (var item in cart.CartItems)
        {
            // Stock validation
            if (item.Quantity > item.Product.Stock)
                throw new Exception(
                    $"Insufficient stock for {item.Product.Name}");

            // Reduce stock
            item.Product.Stock -= item.Quantity;

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.Price
            });
        }

        // 4. Save order
        await _orderRepo.CreateOrderAsync(order);

        // 5. Clear cart
        cart.CartItems.Clear();

        // 6. Save changes
        await _orderRepo.SaveChangesAsync();
    }

    public async Task<List<OrderResponseDTO>> GetMyOrdersAsync(int userId)
    {
        var orders = await _orderRepo.GetUserOrdersAsync(userId);

        return orders.Select(order => new OrderResponseDTO
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedAt = order.CreatedAt,

            Items = order.OrderItems.Select(item =>
                new OrderItemResponseDTO
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()

        }).ToList();
    }
}