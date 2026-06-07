using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces;
using ECommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var result = await _service.RegisterAsync(dto);
        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "User registered successfully",
            Data = result
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await _service.LoginAsync(dto);
        return Ok(new ApiResponse<AuthResponseDTO>
        {
            Success = true,
            Message = "Login successful",
            Data = result
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenDTO dto)
    {
        var result = await _service.RefreshTokenAsync(dto);

        return Ok(result);
    }
}