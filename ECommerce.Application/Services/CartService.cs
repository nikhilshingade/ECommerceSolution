using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IProductRepository _productRepo;

    public CartService(ICartRepository cartRepo,IProductRepository productRepo)
    {
        _cartRepo = cartRepo;
        _productRepo = productRepo;
    }

    public async Task UpdateCartItemAsync( int userId, UpdateCartItemDTO dto)
    {
        if (dto.Quantity < 0)
            throw new Exception("Quantity cannot be negative");

        var cart = await _cartRepo.GetUserCartAsync(userId);

        if (cart == null)
            throw new Exception("Cart not found");

        var cartItem = cart.CartItems
            .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

        if (cartItem == null)
            throw new Exception("Cart item not found");

        // Remove item if quantity becomes zero
        if (dto.Quantity == 0)
        {
            cart.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity = dto.Quantity;
        }

        await _cartRepo.SaveChangesAsync();
    }

    public async Task AddToCartAsync(int userId, AddToCartDTO dto)
    {
        // 1. Check product exists
        var product = await _productRepo.GetByIdAsync(dto.ProductId);

        if (product == null)
            throw new Exception("Product not found");

        // 2. Get user cart
        var cart = await _cartRepo.GetUserCartAsync(userId);

        // 3. Create cart if not exists
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            await _cartRepo.CreateCartAsync(cart);
            await _cartRepo.SaveChangesAsync();
        }

        // 4. Check existing cart item
        var existingItem = cart.CartItems
            .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

        // 5. Increase quantity if exists
        if (existingItem != null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            // 6. Add new item
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _cartRepo.AddCartItemAsync(cartItem);
        }

        await _cartRepo.SaveChangesAsync();
    }

    public async Task<CartResponseDTO?> GetCartAsync(int userId)
    {
        var cart = await _cartRepo.GetUserCartAsync(userId);

        if (cart == null)
            return null;

        var response = new CartResponseDTO();

        response.Items = cart.CartItems.Select(ci => new CartItemResponseDTO
        {
            ProductId = ci.ProductId,
            ProductName = ci.Product.Name,
            Price = ci.Product.Price,
            Quantity = ci.Quantity,
            ImageUrl = ci.Product.ImageUrl
        }).ToList();

        response.TotalAmount = cart.CartItems.Sum(ci =>
            ci.Product.Price * ci.Quantity);

        return response;
    }
}