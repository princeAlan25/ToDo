using System;
using System.Collections.Generic;
using System.Text;
using ToDoEntityModels.Models;

namespace ToDo.Interfaces.IRepositories;

public interface IRole
{
    public Task<Role> CreateRoleAsync(Category role);
    public Task<Role> UpdateRoleAsync(Category role);
    public Task<Role>? GetRoleById(int roleId);
    public Task DeleteRoleAsync(int roleId);
}
