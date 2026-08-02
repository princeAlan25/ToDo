using ToDoApi.DTOs;

namespace ToDoApi.IIntermediators;

public interface ITaskIntermediator
{
    public IResult UpdateTaskAsync(UpdateTaskItemDto task);
    public IResult CreateTaskAsync(CreateTaskItemDto task);
    public IResult GetTaskById(int taskId);
    public IResult GetTasksAsync();
    public IResult DeleteTaskAsync(int taskId);
}
