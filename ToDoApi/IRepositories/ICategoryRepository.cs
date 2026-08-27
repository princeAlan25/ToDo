using System;
using System.Collections.Generic;
using System.Text;
using ToDoShared.DTOs;
using ToDoEntityModels.Models;

namespace ToDoApi.IRepositories;

public interface ICategoryRepository
{
    public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category);
    public Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto category);
    public Task<CategoryDto?> GetCategoryById(int categoryId);
    public Task<List<CategoryDto>?> GetCategoriesAsync();
    public Task<bool> DeleteCategoryAsync(int categoryId);
}
