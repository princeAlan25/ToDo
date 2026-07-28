using ToDoApi.DTOs;
using ToDoApi.IIntermediators;
using ToDoEntityModels.Models;

namespace ToDoApi.EndPoints;

public static partial class Program
{
    public static void MapUserEndPoints(this WebApplication app)
    {
        app.MapGet(
            pattern: "/users/{userId}",
            handler: (IUserIntermediator service, string userId) => service.GetUserByIdAsync(userId));

        app.MapPost(
            pattern: "/users",
            handler: async (IUserIntermediator service, CreateUserDto user) => 
            {
                User createdUser = new()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Password = user.Password
                };
                return service.CreateUserAsync(createdUser);
            });

        app.MapPut(
            pattern: "/users",
            handler: (IUserIntermediator service, User user) => service.UpdateUserAsync(user));

        app.MapDelete(
            pattern: "/users/{userId}",
            handler: (IUserIntermediator service, string userId) => service.DeleteUserAsync(userId));
    }
}
