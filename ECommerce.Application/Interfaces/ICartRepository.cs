using ECommerce.Domain.Entities;

public interface ICartRepository
{
    Task<Cart?> GetUserCartAsync(int userId);

    Task CreateCartAsync(Cart cart);

    Task AddCartItemAsync(CartItem cartItem);

    Task SaveChangesAsync();
}