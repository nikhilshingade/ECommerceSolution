using ECommerce.Application.DTOs.Cart;

public interface ICartService
{
    Task AddToCartAsync(int userId, AddToCartDTO dto);

    Task<CartResponseDTO?> GetCartAsync(int userId);
    Task UpdateCartItemAsync(int userId, UpdateCartItemDTO dto);
}