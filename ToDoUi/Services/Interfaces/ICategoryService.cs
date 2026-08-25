using ToDoShared.DTOs;

namespace ToDoUi.Services.Interfaces;

public interface ICategoryService
{
    public Task<CategoryDto?> CreateCategoryAsync(CreateCategoryDto category);
    public Task<CategoryDto?> UpdateCategoryAsync(UpdateCategoryDto category);
    public Task<CategoryDto?> GetCategoryById(int categoryId);
    public Task<List<CategoryDto>?> GetCategoriesAsync();
    public Task<bool> DeleteCategoryAsync(int categoryId);
}
