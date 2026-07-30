using System;
using System.Collections.Generic;
using System.Text;
using ToDoApi.DTOs;
using ToDoEntityModels.Models;

namespace ToDoApi.IRepositories;

public interface IRoleRepository
{
    public Task<RoleDto> CreateRoleAsync(CreateRoleDto role);
    public Task<RoleDto?>? UpdateRoleAsync(UpdateRoleDto role);
    public Task<RoleDto?> GetRoleById(int roleId);
    public Task<bool> DeleteRoleAsync(int roleId);
    public Task<List<RoleDto>?> GetAllRolesAsync();
}
