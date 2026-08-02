using ToDoEntityModels.Models;

namespace ToDoApi.DTOs;

public record TaskItemDto(
    int? TaskId,
    string Name,
    string? Description,
    TaskRate Starred,
    TaskPriority Priority,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int CategoryId
);

public record CreateTaskItemDto(
    string Name,
    string? Description,
    TaskRate? Starred,
    TaskPriority? Priority,
    int CategoryId
)
{
    public TaskRate? Starred { get; init; } = Starred ?? TaskRate.Low;
    public TaskPriority? Priority { get; init; } = Priority ?? TaskPriority.Low;
}

public record UpdateTaskItemDto(
    int TaskId,
    string Name,
    string? Description,
    TaskRate Starred,
    TaskPriority Priority,
    int CategoryId
)
{
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
}
