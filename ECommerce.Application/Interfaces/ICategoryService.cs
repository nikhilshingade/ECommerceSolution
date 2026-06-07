using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs.Category;

namespace ECommerce.Application.Interfaces;
public interface ICategoryService
{
    Task<List<CategoryResponseDTO>> GetAllAsync();
}
