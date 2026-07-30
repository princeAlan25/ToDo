using Microsoft.EntityFrameworkCore;
using ToDoApi.DTOs;
using ToDoApi.IRepositories;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Repositories;

public class RoleRepo(ToDoContext db, ILogger<RoleRepo> logger) : IRoleRepository
{
    private readonly ToDoContext _db = db;
    public Task<RoleDto> CreateRoleAsync(CreateRoleDto role)
    {
        ArgumentNullException.ThrowIfNull(role);
        Role dbRoleMap = new()
        {
            Name = role.Name
        };
        _db.Roles.Add(dbRoleMap);
        _db.SaveChangesAsync();
        RoleDto roleResult = new()
        {
            RoleId = dbRoleMap.RoleId,
            Name = dbRoleMap.Name,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ReferenceUsersCount = dbRoleMap.Users is not null ? dbRoleMap.Users.Count : 0
        };
        return Task.FromResult(roleResult);
    }

    public Task<bool> DeleteRoleAsync(int roleId)
    {
        Role? role = _db.Roles.FirstOrDefault(r => r.RoleId == roleId);
        if (role == null) 
        {
            logger.LogError("Unknown Role Database record", [ role ]);
            return Task.FromResult(false);
        }
        _db.Roles.Remove(role);
        _db.SaveChangesAsync();
        return Task.FromResult(true);
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

    public Task<RoleDto?> UpdateRoleAsync(UpdateRoleDto role)
    {
        ArgumentNullException.ThrowIfNull(role);
        try
        {
            Role roleMap = new()
            {
                Name = role.Name
            };
            _db.Entry<Role>(roleMap).CurrentValues.SetValues(roleMap);
            _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message, [ex]);
        }
        
        Role? updatedRole = _db.Roles.Include(r => r.Users).FirstOrDefault(r => r.Name == role.Name) ?? throw new InvalidOperationException(message: "Invalid database updated operation");
        RoleDto updatedRoleMap = new()
        {
            RoleId = updatedRole.RoleId,
            Name = updatedRole.Name,
            CreatedAt = updatedRole.CreatedAt,
            UpdatedAt = DateTime.Now,
            ReferenceUsersCount = updatedRole.Users is not null ? updatedRole.Users.Count : 0
        };
        return Task.FromResult(updatedRoleMap ?? null);
    }
}
