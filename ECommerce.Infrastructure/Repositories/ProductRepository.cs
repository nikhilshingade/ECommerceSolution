using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.DTOs.Product;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
        .Where(p => !p.IsDeleted)
        .Include(p => p.Category)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Product>> GetPagedProductsAsync(ProductQueryParameters query)
    {
        var productsQuery = _context.Products
        .Where(p => !p.IsDeleted)
        .Include(p => p.Category)
        .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            productsQuery = productsQuery.Where(p =>
                p.Name.ToLower().Contains(query.Search.ToLower()));
        }

        // Category Filter
        if (query.CategoryId.HasValue)
        {
            productsQuery = productsQuery.Where(p =>
                p.CategoryId == query.CategoryId.Value);
        }

        // Pagination
        productsQuery = productsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);

        return await productsQuery.ToListAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        product.IsDeleted = true;

        _context.Products.Update(product);

        await _context.SaveChangesAsync();
    }
}
