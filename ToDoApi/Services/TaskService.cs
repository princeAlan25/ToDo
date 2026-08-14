using ToDoShared.DTOs;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;

namespace ToDoApi.Services;

public class TaskService(ITaskRepository taskRepo) : ITaskIntermediator
{
    private readonly ITaskRepository _taskRepo = taskRepo;
    public IResult CreateTaskAsync(CreateTaskItemDto task)
    {
        if(task == null) return Results.BadRequest("Task cannot be null.");
        TaskItemDto? createdTask = _taskRepo.CreateTaskAsync(task).Result;
        return Results.Created($"/tasks/{task.Name}", createdTask);
    }

    public IResult DeleteTaskAsync(int taskId)
    {
        return _taskRepo.DeleteTaskAsync(taskId).Result
            ? Results.Ok($"Task with ID {taskId} deleted successfully.")
            : Results.Problem($"Failed to delete task with ID {taskId}. or Task with ID {taskId} not found");
    }

    public IResult GetTaskById(int taskId)
    {
        TaskItemDto? task = _taskRepo.GetTaskById(taskId).Result;
        return task != null
            ? Results.Ok<TaskItemDto>(task)
            : Results.NotFound($"Task with ID {taskId} not found.");
    }

    public IResult GetTasksAsync()
    {
        return Results.Ok<List<TaskItemDto>>(_taskRepo.GetTasksAsync().Result);
    }

    public IResult UpdateTaskAsync(UpdateTaskItemDto task)
    {
        if(task == null) return Results.BadRequest("Task cannot be null.");
        TaskItemDto? updatedTask = _taskRepo.UpdateTaskAsync(task).Result;
        return updatedTask != null
            ? Results.Ok<TaskItemDto>(updatedTask)
            : Results.NotFound($"Task with ID {task.TaskId} not found.");
    }
}
