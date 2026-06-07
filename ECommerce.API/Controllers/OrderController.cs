using System.Security.Claims;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using ECommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpPost("place")]
    public async Task<IActionResult> PlaceOrder()
    {
        var userId = int.Parse(
            User.FindFirst("UserId")!.Value);

        await _service.PlaceOrderAsync(userId);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Order placed successfully",
            Data = null
        });
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = int.Parse(
            User.FindFirst("UserId")!.Value);

        var orders = await _service.GetMyOrdersAsync(userId);

        return Ok(new ApiResponse<List<OrderResponseDTO>>
        {
            Success = true,
            Message = "Orders fetched successfully",
            Data = orders
        });
    }
}