namespace ECommerce.Application.DTOs.Cart;

public class CartResponseDTO
{
    public List<CartItemResponseDTO> Items { get; set; } = new();

    public decimal TotalAmount { get; set; }
}