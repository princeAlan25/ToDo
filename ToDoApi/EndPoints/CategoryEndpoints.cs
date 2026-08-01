using ToDoApi.DTOs;
using ToDoApi.IIntermediators;

namespace ToDoApi.EndPoints;

public static partial class Program
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routebuilder = app.MapGroup("categories").WithTags("Categories Endpoints");
        routebuilder.MapPost("/", async (CreateCategoryDto category, ICategoryIntermediator categoryIntermediator) =>
        {
            return categoryIntermediator.CreateCategoryAsync(category);
        }).RequireAuthorization("authenticated").WithName("CreateCategory Endpoint");

        routebuilder.MapPut("/", async (UpdateCategoryDto category, ICategoryIntermediator categoryIntermediator) =>
        {
            return categoryIntermediator.UpdateCategoryAsync(category);
        }).RequireAuthorization("authenticated").WithName("UpdateCategory Endpoint");

        routebuilder.MapGet("/{categoryId:int}", async (int categoryId, ICategoryIntermediator categoryIntermediator) =>
        {
            return categoryIntermediator.GetCategoryById(categoryId);
        }).RequireAuthorization("authenticated").WithName("GetCategory Endpoint");

        routebuilder.MapGet("/", async (ICategoryIntermediator categoryIntermediator) =>
        {
            return categoryIntermediator.GetCategoriesAsync();
        }).RequireAuthorization("authenticated").WithName("GetCategories Endpoint");

        routebuilder.MapDelete("/{categoryId:int}", async (int categoryId, ICategoryIntermediator categoryIntermediator) =>
        {
            return categoryIntermediator.DeleteCategoryAsync(categoryId);
        }).RequireAuthorization("authenticated").WithName("DeleteCategory Endpoint");
    }
}
