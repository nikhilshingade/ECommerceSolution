using ECommerce.Application.DTOs.Product;

public interface IProductService
{
    Task CreateAsync(CreateProductDTO dto);

    Task<List<ProductResponseDTO>> GetAllAsync();

    Task<ProductResponseDTO?> GetByIdAsync(int id);

    Task<List<ProductResponseDTO>> GetPagedAsync(ProductQueryParameters query);

    Task UpdateAsync(int id, UpdateProductDTO dto);

    Task DeleteAsync(int id);
}