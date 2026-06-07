using ECommerce.Application.DTOs.Product;
using ECommerce.Domain.Entities;

public interface IProductRepository
{
    Task AddAsync(Product product);

    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<List<Product>> GetPagedProductsAsync(ProductQueryParameters query);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);
}