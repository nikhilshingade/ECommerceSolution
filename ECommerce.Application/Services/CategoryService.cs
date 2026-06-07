using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs.Category;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryResponseDTO>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync();

        return categories.Select(c => new CategoryResponseDTO
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }
}
