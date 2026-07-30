using ToDoApi.DTOs;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;

namespace ToDoApi.Services;

public class RoleService(IRoleRepository roleRepo) : IRoleIntermediator
{
    private readonly IRoleRepository _roleRepo = roleRepo;
    public IResult CreateRoleAsync(CreateRoleDto role)
    {
        if (role == null) return Results.BadRequest("Invalid Role request");
        RoleDto createdRole = _roleRepo.CreateRoleAsync(role).Result;
        return Results.Created("/role", createdRole);
    }

    public IResult DeleteRoleAsync(int roleId)
    {
        bool roleDeleted = _roleRepo.DeleteRoleAsync(roleId).Result;
        if (!roleDeleted) return Results.BadRequest($"Role {roleId} does not exist");
        return Results.Accepted("/role", roleDeleted);
    }

    public IResult GetAllRolesAsync()
    {
        List<RoleDto>? roles = _roleRepo.GetAllRolesAsync().Result;
        return Results.Ok<List<RoleDto>>(roles);
    }

    public IResult GetRoleById(int roleId)
    {
        RoleDto? role = _roleRepo.GetRoleById(roleId).Result;
        if (role == null) return Results.BadRequest($"Unknown Role {roleId}");
        return Results.Ok<RoleDto>(role);
    }

    public IResult UpdateRoleAsync(UpdateRoleDto role)
    {
        if (role == null) return Results.BadRequest($"Invalid Request {role}");
        RoleDto? updatedRole = _roleRepo.UpdateRoleAsync(role)?.Result;
        if (updatedRole == null) return Results.BadRequest($"{ HttpRequestError.InvalidResponse }. Invalid Role Updating Operation");
        return Results.Accepted<RoleDto>("/user", updatedRole);
    }
}
