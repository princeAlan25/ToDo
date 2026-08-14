using Microsoft.EntityFrameworkCore;
using ToDoShared.DTOs;
using ToDoApi.IRepositories;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Repositories;

public class RoleRepo(ToDoContext db, ILogger<RoleRepo> logger) : IRoleRepository
{
    private readonly ToDoContext _db = db;
    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto role)
    {
        ArgumentNullException.ThrowIfNull(role);
        Role dbRoleMap = new()
        {
            Name = role.Name
        };

        _db.Roles.Add(dbRoleMap);
        await _db.SaveChangesAsync();

        RoleDto roleResult = new()
        {
            RoleId = dbRoleMap.RoleId,
            Name = dbRoleMap.Name,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ReferenceUsersCount = dbRoleMap.Users is not null ? dbRoleMap.Users.Count : 0
        };
        return roleResult;
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        Role? role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        if (role == null) 
        {
            logger.LogError("Unknown Role Database record", [ role ]);
            return false;
        }
        _db.Roles.Remove(role);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<RoleDto>?> GetAllRolesAsync()
    {
        List<RoleDto> roles = await _db.Roles.Include(r => r.Users)
            .Select(r => new RoleDto { 
                RoleId = r.RoleId,
                Name = r.Name,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ReferenceUsersCount = r.Users != null ? r.Users.Count : 0
            }).ToListAsync();
        return roles;
    }

    public async Task<RoleDto?> GetRoleById(int roleId)
    {
        Role? foundRole = await _db.Roles.Include(r => r.Users).FirstOrDefaultAsync(r => r.RoleId == roleId);
        if (foundRole == null) return null;
        RoleDto role = new()
        {
            RoleId = roleId,
            Name = foundRole.Name,
            CreatedAt = foundRole.CreatedAt,
            UpdatedAt = foundRole.UpdatedAt,
            ReferenceUsersCount = foundRole.Users is not null ? foundRole.Users.Count : 0
        };
        return role;
    }

    public async Task<RoleDto?> UpdateRoleAsync(UpdateRoleDto role)
    {
        ArgumentNullException.ThrowIfNull(role);

        await _db.Roles
            .Where(r => r.RoleId == role.RoleId)
            .ExecuteUpdateAsync(r => r
                .SetProperty(r => r.Name, role.Name)
                .SetProperty(r => r.UpdatedAt, role.UpdatedAt)
            );

        RoleDto? updatedRole = await _db.Roles
            .AsNoTracking()
            .Where(r => r.RoleId == role.RoleId)
            .Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                Name = r.Name,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                ReferenceUsersCount = r.Users != null ? r.Users.Count : 0
            })
            .FirstOrDefaultAsync();

        return updatedRole;
    }
}
