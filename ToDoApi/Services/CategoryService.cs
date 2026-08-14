using ToDoShared.DTOs;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;

namespace ToDoApi.Services;

public class CategoryService(ICategoryRepository categoryRepo) : ICategoryIntermediator
{
    private readonly ICategoryRepository _categoryRepo = categoryRepo;
    public IResult CreateCategoryAsync(CreateCategoryDto category)
    {
        ArgumentNullException.ThrowIfNull(category, nameof(category));
        CategoryDto createdCategory = _categoryRepo.CreateCategoryAsync(category).Result;
        if (createdCategory is null) return Results.BadRequest("Failed to create category.");
        return Results.Created($"/api/categories/{createdCategory?.CategoryId}", createdCategory);
    }

    public IResult DeleteCategoryAsync(int categoryId)
    {
        bool categoryDeleted = _categoryRepo.DeleteCategoryAsync(categoryId).Result;
        if (!categoryDeleted) return Results.NotFound($"Category with ID {categoryId} not found.");
        return Results.Content($"Category {categoryId} deleted successfully.");
    }

    public IResult GetCategoriesAsync()
    {
        List<CategoryDto>? categories = _categoryRepo.GetCategoriesAsync()?.Result;
        return Results.Ok(categories);
    }

    public IResult GetCategoryById(int categoryId)
    {
        CategoryDto? category = _categoryRepo.GetCategoryById(categoryId)?.Result;
        if (category == null) return Results.NotFound($"Category with ID {categoryId} not found.");
        return Results.Ok(category);
    }

    public IResult UpdateCategoryAsync(UpdateCategoryDto category)
    {
        CategoryDto? updatedCategory = _categoryRepo.UpdateCategoryAsync(category)?.Result;
        if (updatedCategory == null) return Results.NotFound($"Category with ID {category.CategoryId} not found.");
        return Results.Ok(updatedCategory);
    }
}
