using ToDoEntityModels.Models;

namespace ToDoShared.DTOs;

public record CategoryDto(
    int? CategoryId,
    string Name,
    string ColorCode,
    string Description,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<TaskItem> Tasks
) {
    List<TaskItem> Tasks { get; init; } = Tasks ?? [];
    int ReferencedTasksCount => Tasks.Count;
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
