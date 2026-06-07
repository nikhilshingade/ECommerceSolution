using ECommerce.Application.DTOs.Category;
using ECommerce.Application.Interfaces;
using ECommerce.Shared;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();

        return Ok(new ApiResponse<List<CategoryResponseDTO>>
        {
            Success = true,
            Message = "Categories fetched successfully",
            Data = categories
        });
    }
}