using ToDoShared.DTOs;
using ToDoUi.Networking.Interfaces;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.Services.Implementations;

public class CategoryService(IApiClient apiClient) : ICategoryService
{
    private readonly IApiClient _apiClient = apiClient;

    public async Task<CategoryDto?> CreateCategoryAsync(CreateCategoryDto category)
    {
        return await _apiClient.PostAsync<CreateCategoryDto, CategoryDto>("/users",category);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        return await _apiClient.DeleteAsync<int, bool>("/users", categoryId);
    }

    public async Task<List<CategoryDto>?> GetCategoriesAsync()
    {
        return await _apiClient.GetAsync<List<CategoryDto>>("/users");
    }

    public async Task<CategoryDto?> GetCategoryById(int categoryId)
    {
        return await _apiClient.GetAsync<CategoryDto>($"/users/{categoryId}");
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(UpdateCategoryDto category)
    {
        return await _apiClient.PutAsync<UpdateCategoryDto, CategoryDto>("/users", category);
    }
}
