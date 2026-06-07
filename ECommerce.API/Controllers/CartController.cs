using System.Security.Claims;
using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Interfaces;
using ECommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(AddToCartDTO dto)
    {
        var userId = int.Parse(
            User.FindFirst("UserId")!.Value);

        await _service.AddToCartAsync(userId, dto);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Item added to cart",
            Data = null
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCartItem(UpdateCartItemDTO dto)
    {
        var userId = int.Parse(User.FindFirst("UserId")!.Value);

        await _service.UpdateCartItemAsync(userId,dto);

        return Ok("Cart updated");
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = int.Parse(
            User.FindFirst("UserId")!.Value);

        var cart = await _service.GetCartAsync(userId);

        return Ok(new ApiResponse<CartResponseDTO>
        {
            Success = true,
            Message = "Cart fetched successfully",
            Data = cart
        });
    }
}