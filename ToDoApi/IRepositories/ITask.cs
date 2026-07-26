using System;
using System.Collections.Generic;
using System.Text;
using ToDoEntityModels.Models;

namespace ToDo.Interfaces.IRepositories;

public interface ITask
{
    public Task<TaskItem> UpdateTaskAsync(TaskItem task);
    public Task<TaskItem> CreateTaskAsync(TaskItem task);
    public Task<TaskItem>? GetTaskById(int taskId);
    public Task<List<TaskItem>>? GetTasksByCategory(string category);
    public Task<List<TaskItem>>? GetTasksAsync();
    public Task DeleteTaskAsync(int taskId);
}
