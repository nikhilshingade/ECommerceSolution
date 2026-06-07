using System.Data;
using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities;

public class User:BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }
}