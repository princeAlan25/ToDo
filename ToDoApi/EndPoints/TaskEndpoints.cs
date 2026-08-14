using ToDoShared.DTOs;
using ToDoApi.IIntermediators;

namespace ToDoApi.EndPoints;

public static partial class Program
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        RouteGroupBuilder routeBuilder = app.MapGroup("/tasks").WithTags("Task Endpoints");

        routeBuilder.MapGet("/", async (ITaskIntermediator taskIntermediator) =>
        {
            return taskIntermediator.GetTasksAsync();
        }).RequireAuthorization("authenticated").WithName("GetTasks Endpoint");

        routeBuilder.MapGet("/{taskId:int}", async (int taskId, ITaskIntermediator taskIntermediator) =>
        {
            return taskIntermediator.GetTaskById(taskId);
        }).RequireAuthorization("authenticated").WithName("GetTaskById Endpoint");

        routeBuilder.MapPost("/", async (CreateTaskItemDto task, ITaskIntermediator taskIntermediator) =>
        {
            return taskIntermediator.CreateTaskAsync(task);
        }).RequireAuthorization("authenticated").WithName("CreateTask Endpoint");

        routeBuilder.MapPut("/", async (UpdateTaskItemDto task, ITaskIntermediator taskIntermediator) =>
        {
            return taskIntermediator.UpdateTaskAsync(task);
        }).RequireAuthorization("authenticated").WithName("UpdateTask Endpoint");

        routeBuilder.MapDelete("/{taskId:int}", async (int taskId, ITaskIntermediator taskIntermediator) =>
        {
            return taskIntermediator.DeleteTaskAsync(taskId);
        }).RequireAuthorization("authenticated").WithName("DeleteTask Endpoint");
    }
}
