using ToDoApi.DTOs;

namespace ToDoApi.IIntermediators;

public interface ICategoryIntermediator
{
    public IResult CreateCategoryAsync(CreateCategoryDto category);
    public IResult UpdateCategoryAsync(UpdateCategoryDto category);
    public IResult GetCategoryById(int categoryId);
    public IResult GetCategoriesAsync();
    public IResult DeleteCategoryAsync(int categoryId);
}
