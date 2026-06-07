using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces;
using ECommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    // Public API
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllAsync();
        return Ok(new ApiResponse<List<ProductResponseDTO>>
        {
            Success = true,
            Message = "Products fetched successfully",
            Data = products
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product == null)
            return NotFound("Product not found");

        return Ok(new ApiResponse<ProductResponseDTO>
        {
            Success = true,
            Message = "Product fetched successfully",
            Data = product
        });
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] ProductQueryParameters query)
    {
        var result = await _service.GetPagedAsync(query);

        return Ok(new ApiResponse<List<ProductResponseDTO>>
        {
            Success = true,
            Message = "Paged products fetched successfully",
            Data = result
        });
    }
//--------------------------------------------------------------------------------------------------------------------
    // Admin Only
    // Add Products 
    [Authorize(Roles = "Admin")]  
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDTO dto)
    {
        await _service.CreateAsync(dto);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Product created successfully",
            Data = null
        });
    }

    // Update Products 
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDTO dto)
    {
        await _service.UpdateAsync(id, dto);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Product updated successfully",
            Data = null
        });
    }

    // Delete Product
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Product deleted successfully",
            Data = null
        });
    }


}