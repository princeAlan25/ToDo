using ToDoApi.DTOs;

namespace ToDoApi.IRepositories;

public interface ITaskRepository
{
    public Task<TaskItemDto?> UpdateTaskAsync(UpdateTaskItemDto task);
    public Task<TaskItemDto?> CreateTaskAsync(CreateTaskItemDto task);
    public Task<TaskItemDto?> GetTaskById(int taskId);
    public Task<List<TaskItemDto>?> GetTasksAsync();
    public Task<bool> DeleteTaskAsync(int taskId);
}
