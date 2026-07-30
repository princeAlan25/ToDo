using ToDoApi.DTOs;

namespace ToDoApi.IIntermediators;

public interface IRoleIntermediator
{
    public IResult CreateRoleAsync(CreateRoleDto role);
    public IResult UpdateRoleAsync(UpdateRoleDto role);
    public IResult GetRoleById(int roleId);
    public IResult DeleteRoleAsync(int roleId);
    public IResult GetAllRolesAsync();
}
