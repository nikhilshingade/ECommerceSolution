using ECommerce.Domain.Entities;

public interface IOrderRepository
{
    Task CreateOrderAsync(Order order);

    Task<List<Order>> GetUserOrdersAsync(int userId);

    Task SaveChangesAsync();
}