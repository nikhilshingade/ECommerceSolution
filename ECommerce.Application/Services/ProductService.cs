using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Logging;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ILogger<ProductService> _logger;
    public ProductService(IProductRepository productRepo, ILogger<ProductService> logger)
    {
        _productRepo = productRepo;
        _logger = logger;
    }

    //To create new product
    public async Task CreateAsync(CreateProductDTO dto)
    {
        _logger.LogInformation("Creating product: {ProductName}", dto.Name);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            ImageUrl = dto.ImageUrl
        };

        await _productRepo.AddAsync(product);
    }

    // To get the list of products 
    public async Task<List<ProductResponseDTO>> GetAllAsync()
    {
        var products = await _productRepo.GetAllAsync();

        return products.Select(p => new ProductResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category?.Name ?? "",
            ImageUrl = p.ImageUrl
        }).ToList();
    }

    // Find product by id 
    public async Task<ProductResponseDTO?> GetByIdAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
            return null;

        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryName = product.Category.Name,
            ImageUrl = product.ImageUrl
        };
    }

    // Pagination search and filter 
    public async Task<List<ProductResponseDTO>> GetPagedAsync(ProductQueryParameters query)
    {
        var products = await _productRepo.GetPagedProductsAsync(query);

        return products.Select(p => new ProductResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl
        }).ToList();
    }

    // To update the product 
    public async Task UpdateAsync(int id, UpdateProductDTO dto)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
            throw new Exception("Product not found");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;
        product.ImageUrl = dto.ImageUrl;

        await _productRepo.UpdateAsync(product);
    }

    // To delete the product 
    public async Task DeleteAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
            throw new Exception("Product not found");

        await _productRepo.DeleteAsync(product);
    }
}