using ECommerce.Application.DTOs.Order;

public interface IOrderService
{
    Task PlaceOrderAsync(int userId);

    Task<List<OrderResponseDTO>> GetMyOrdersAsync(int userId);
}