using ToDoEntityModels.Models;

namespace ToDoShared.DTOs;

using System.Text.Json.Serialization;

//[method: JsonConstructor]
public class CategoryDto(int? categoryId, string name, string colorCode, string description, DateTime createdAt, DateTime updatedAt, List<TaskItem> tasks)
{
    public int? CategoryId { get; set; } = categoryId;
    public string Name { get; set; } = name;
    public string ColorCode { get; set; } = colorCode;
    public string Description { get; set; } = description;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
    public List<TaskItem> Tasks { get; set; } = tasks ?? [];
    public int ReferencedTasksCount => Tasks?.Count ?? 0;
}

public record CreateCategoryDto(
    string Name,
    string ColorCode,
    string Description
);

public record UpdateCategoryDto(
    int CategoryId,
    string Name,
    string ColorCode,
    string Description
);
