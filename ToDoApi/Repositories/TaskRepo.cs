using Microsoft.EntityFrameworkCore;
using ToDoShared.DTOs;
using ToDoApi.IRepositories;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Repositories;

public class TaskRepo(ToDoContext db) : ITaskRepository
{
    private readonly ToDoContext _db = db;
    public async Task<TaskItemDto?> CreateTaskAsync(CreateTaskItemDto task)
    {
        ArgumentNullException.ThrowIfNull(task);
        try
        {
            bool taskExists = await _db.Tasks.AnyAsync(t => t.Name == task.Name);
            if (taskExists) return null;

            TaskItem? taskItem = new()
            {
                Name = task.Name,
                Description = task.Description,
                CategoryId = task.CategoryId,
                Starred = task.Starred ?? TaskRate.Low,
                Priority = task.Priority ?? TaskPriority.Low,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Tasks.AddAsync(taskItem);
            await _db.SaveChangesAsync();

            taskItem = await _db.Tasks.FirstOrDefaultAsync(t => t.Name == task.Name);
            if (taskItem == null) return null;

            return new TaskItemDto(
                taskItem.TaskId,
                taskItem.Name,
                taskItem.Description,
                taskItem.Starred,
                taskItem.Priority,
                taskItem.CreatedAt,
                taskItem.UpdatedAt,
                taskItem.CategoryId
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the task: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteTaskAsync(int taskId)
    {
        bool taskDeleted = false;
        await Task.Run(async () =>
        {
            TaskItem? task = _db.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task != null)
            {
                _db.Tasks.Remove(task);
                await _db.SaveChangesAsync();
                taskDeleted = true;
            }
        });
        return taskDeleted;
    }

    public async Task<TaskItemDto?> GetTaskById(int taskId)
    {
        TaskItem? taskItemResult = await _db.Tasks
            .Include(t => t.CategoryId)
            .FirstOrDefaultAsync(t => t.TaskId == taskId);
        if (taskItemResult == null) return null;

        return new TaskItemDto(
            taskItemResult.CategoryId,
            taskItemResult.Name,
            taskItemResult.Description,
            taskItemResult.Starred,
            taskItemResult.Priority,
            taskItemResult.CreatedAt,
            taskItemResult.UpdatedAt,
            taskItemResult.CategoryId
        );
    }

    public async Task<List<TaskItemDto>?> GetTasksAsync()
    {
        List<TaskItem> tasks = await _db.Tasks.ToListAsync();
        return [..tasks.Select(t => new TaskItemDto(
            t.TaskId,
            t.Name,
            t.Description,
            t.Starred,
            t.Priority,
            t.CreatedAt,
            t.UpdatedAt,
            t.CategoryId
        ))];
    }

    public async Task<TaskItemDto?> UpdateTaskAsync(UpdateTaskItemDto task)
    {
        await _db.Tasks
            .Where(t => t.TaskId == task.TaskId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(t => t.Name, task.Name)
                .SetProperty(t => t.Description, task.Description)
                .SetProperty(t => t.Starred, task.Starred)
                .SetProperty(t => t.Priority, task.Priority)
                .SetProperty(t => t.CategoryId, task.CategoryId)
                .SetProperty(t => t.UpdatedAt, task.UpdatedAt));

        TaskItemDto? updatedTask = await _db.Tasks
            .AsNoTracking()
            .Where(t => t.TaskId == task.TaskId)
            .Select(t => new TaskItemDto(
                t.TaskId,
                t.Name,
                t.Description,
                t.Starred,
                t.Priority,
                t.CreatedAt,
                t.UpdatedAt,
                t.CategoryId
            ))
            .FirstOrDefaultAsync();

        return updatedTask ?? null;
    }
}
