using System.Security.Claims;
using ToDoApi.DTOs;
using ToDoApi.IIntermediators;
using ToDoEntityModels.Models;

namespace ToDoApi.EndPoints;

public static partial class Program
{
    public static void MapUserEndPoints(this WebApplication app)
    {
        RouteGroupBuilder userEndPointBuilder = app.MapGroup("/user").WithTags("User Management EndPoints");
        userEndPointBuilder.MapGet(
            pattern: "/",
            handler: (IUserIntermediator service, ClaimsPrincipal user) =>
            {
                string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId)) return Results.Unauthorized();
                return service.GetUserByIdAsync(userId);
            })
            .RequireAuthorization("authenticated")
            .WithName("Get User By Id");

        userEndPointBuilder.MapPost(
            pattern: "/",
            handler: async (IUserIntermediator service, CreateUserDto user) =>
            {
                return service.CreateUserAsync(user);
            }).WithName("Create User");

        userEndPointBuilder.MapPut(
            pattern: "/",
            handler: (IUserIntermediator service, UpdateUserDto user) => service.UpdateUserAsync(user))
            .RequireAuthorization("authenticated")
            .WithName("Update User");

        userEndPointBuilder.MapDelete(
            pattern: "/{userId}",
            handler: (IUserIntermediator service, string userId) => service.DeleteUserAsync(userId))
            .RequireAuthorization("authenticated")
            .WithName("Delete User");
    }
}
