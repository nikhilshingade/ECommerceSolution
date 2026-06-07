using ECommerce.Domain.Entities;

public interface IJwtService
{
    string GenerateToken(User user);

    string GenerateRefreshToken();
}