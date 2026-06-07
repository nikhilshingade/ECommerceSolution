namespace ECommerce.Application.DTOs.Order;

public class OrderResponseDTO
{
    public int OrderId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public List<OrderItemResponseDTO> Items { get; set; }
        = new();
}