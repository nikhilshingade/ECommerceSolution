using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}