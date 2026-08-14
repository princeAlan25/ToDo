using Microsoft.EntityFrameworkCore;
using ToDoShared.DTOs;
using ToDoApi.IRepositories;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Repositories
{
    public class CategoryRepo(ToDoContext db) : ICategoryRepository
    {
        private readonly ToDoContext _db = db;

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category)
        {
            Category categoryRequest = new()
            {
                Name = category.Name,
                ColorCode = category.ColorCode,
                Description = category.Description
            };

            _db.Categories.Add(categoryRequest);
            await _db.SaveChangesAsync();

            CategoryDto categoryResponse = new(
                CategoryId: categoryRequest.CategoryId,
                Name: categoryRequest.Name,
                ColorCode: categoryRequest.ColorCode,
                Description: categoryRequest.Description,
                CreatedAt: categoryRequest.CreatedAt,
                UpdatedAt: categoryRequest.UpdatedAt,
                Tasks: []
            );
            return await Task.FromResult(categoryResponse);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            bool isDeleted = false;
            await Task.Run(async () =>
            {
                Category? category = _db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
                if (category != null)
                {
                    _db.Categories.Remove(category);
                    await _db.SaveChangesAsync();
                    isDeleted = true;
                }
            });
            return isDeleted;
        }

        public async Task<List<CategoryDto>>? GetCategoriesAsync()
        {
            List<Category> categories = await _db.Categories.Include(c => c.Tasks).ToListAsync();
            return [..categories.Select(c => new CategoryDto(
                CategoryId: c.CategoryId,
                Name: c.Name,
                ColorCode: c.ColorCode,
                Description: c.Description!,
                CreatedAt: c.CreatedAt,
                UpdatedAt: c.UpdatedAt,
                Tasks: [..c.Tasks]
            ))];
        }

        public Task<CategoryDto>? GetCategoryById(int categoryId)
        {
            Category? category = _db.Categories.Include(c => c.Tasks).FirstOrDefault(c => c.CategoryId == categoryId);
            if (category == null)
                return null;

            CategoryDto categoryResponse = new(
                CategoryId: category.CategoryId,
                Name: category.Name,
                ColorCode: category.ColorCode,
                Description: category.Description!,
                CreatedAt: category.CreatedAt,
                UpdatedAt: category.UpdatedAt,
                Tasks: [..category.Tasks]
            );
            return Task.FromResult(categoryResponse);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto category)
        {
            await _db.Categories
                .Where(c => c.CategoryId == category.CategoryId)
                .ExecuteUpdateAsync(c => c
                    .SetProperty(c => c.Name, category.Name)
                    .SetProperty(c => c.ColorCode, category.ColorCode)
                    .SetProperty(c => c.Description, category.Description)
                    .SetProperty(c => c.UpdatedAt, DateTime.Now));

            Category? updatedCategory = await _db.Categories.Include(c => c.Tasks).FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            return updatedCategory == null
                ? throw new InvalidOperationException("Category to be updated not found or Updating ")
                : await Task.FromResult(new CategoryDto(
                CategoryId: updatedCategory.CategoryId,
                Name: updatedCategory.Name,
                ColorCode: updatedCategory.ColorCode,
                Description: updatedCategory.Description!,
                CreatedAt: updatedCategory.CreatedAt,
                UpdatedAt: updatedCategory.UpdatedAt,
                Tasks: [..updatedCategory.Tasks]
            ));
        }
    }
}
