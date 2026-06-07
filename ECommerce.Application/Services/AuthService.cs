using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using BCrypt.Net;


namespace ECommerce.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<string> RegisterAsync(RegisterDTO dto)
    {
        // 1. Check existing user
        var existingUser = await _userRepo.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new Exception("Email already exists");

        // 2. Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 3. Create user
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            RoleId = 2
        };

        // 4. Save
        await _userRepo.AddUserAsync(user);

        return "User registered successfully";
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new Exception("Invalid credentials");

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isValid)
            throw new Exception("Invalid credentials");

        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(7);

        await _userRepo.SaveChangesAsync();

        return new AuthResponseDTO
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            Role = user.Role.Name
        };
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(
    RefreshTokenDTO dto)
    {
        // 1. Find user using refresh token
        var user = await _userRepo.GetByRefreshTokenAsync(dto.RefreshToken);

        // 2. Validate token
        if (user == null)
            throw new Exception("Invalid refresh token");

        // 3. Check expiry
        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            throw new Exception("Refresh token expired");

        // 4. Generate new access token
        var accessToken = _jwtService.GenerateToken(user);

        // 5. Generate new refresh token
        var newRefreshToken =
            _jwtService.GenerateRefreshToken();

        // 6. Update user token
        user.RefreshToken = newRefreshToken;

        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(7);

        // 7. Save database changes
        await _userRepo.SaveChangesAsync();

        // 8. Return response
        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            Email = user.Email,
            Role = user.Role.Name
        };
    }
}