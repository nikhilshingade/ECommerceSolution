namespace ECommerce.Application.DTOs.Cart;

public class CartItemResponseDTO
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
}