using ToDoApi.DTOs;
using ToDoApi.IIntermediators;

namespace ToDoApi.EndPoints;

public static partial class Program
{
    public static void MapRoleEndPoints(this WebApplication app)
    {
        RouteGroupBuilder routeBuilder = app.MapGroup("/role").WithTags("Role Endpoints");

        routeBuilder.MapPost(
            pattern: "/",
            handler: (IRoleIntermediator roleService, CreateRoleDto role) => roleService.CreateRoleAsync(role)
            ).RequireAuthorization("authenticated").WithName("Create Role Endpoint");

        routeBuilder.MapPut(
            pattern: "/",
            handler: (IRoleIntermediator roleService, UpdateRoleDto role) => roleService.UpdateRoleAsync(role)
            ).RequireAuthorization("authenticated").WithName("Update Role Endpoint");

        routeBuilder.MapDelete(
            pattern: "/{roleId}",
            handler: (IRoleIntermediator roleService, int roleId) => roleService.DeleteRoleAsync(roleId)
            ).RequireAuthorization("authenticated").WithName("Delete Role Endpoint");

        routeBuilder.MapGet(
            pattern: "/{roleId}",
            handler: (IRoleIntermediator roleService, int roleId) => roleService.GetRoleById(roleId)
            ).RequireAuthorization("authenticated").WithName("Get Role by ID Endpoint");

        routeBuilder.MapGet(
            pattern: "/",
            handler: (IRoleIntermediator roleService) => roleService.GetAllRolesAsync()
            ).RequireAuthorization("authenticated").WithName("Get all Roles Endpoint");
    }
}
