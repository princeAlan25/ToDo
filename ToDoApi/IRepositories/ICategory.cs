using System;
using System.Collections.Generic;
using System.Text;
using ToDoEntityModels.Models;

namespace ToDoApi.IRepositories;

public interface ICategory
{
    public Task<Category> CreateCategoryAsync(Category category);
    public Task<Category> UpdateCategoryAsync(Category category);
    public Task<Category>? GetCategoryById(int categoryId);
    public Task<List<Category>>? GetCategoriesByUser(int userId);
    public Task<List<Category>>? GetCategoriesAsync();
    public Task DeleteCategoryAsync(int categoryId);
}
