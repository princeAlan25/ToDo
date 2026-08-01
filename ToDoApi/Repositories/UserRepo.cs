using Microsoft.EntityFrameworkCore;
using ToDoApi.DTOs;
using ToDoApi.IRepositories;
using ToDoApi.Utilities;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Repositories;

public class UserRepo(ToDoContext db) : IUserRepository
{
    private readonly ToDoContext _db = db;
    public Task<UserDto> CreateUserAsync(CreateUserDto user)
    {
        ArgumentNullException.ThrowIfNull(user);
        User userRequest = new()
        {
            UserId = Guid.NewGuid(),
            Email = user.Email,
            Name = user.Name,
            Password = Securit.HashPassword(user.Password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _db.Users.AddAsync(userRequest);
        _db.SaveChangesAsync();

        UserDto createdUser = new(
            userRequest.UserId,
            userRequest.Email,
            userRequest.Name,
            userRequest.Role,
            [.. userRequest.Categories],
            userRequest.CreatedAt,
            userRequest.UpdatedAt
            );
        return Task.FromResult(createdUser);
    }

    public Task<bool> DeleteUserAsync(Guid userId)
    {
        if(userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        User userExist = _db.Users.First(u => u.UserId == userId);
        if (userExist == null)
        {
            return Task.FromResult(false);
        }
        _db.Users.Remove(userExist);
        _db.SaveChangesAsync();
        return Task.FromResult(true);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(email);
        User? foundUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        return foundUser ?? null;
    }

    public async Task<UserDto?> GetUserById(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        User? foundUser = await _db.Users.Include(u => u.Role).Include(u => u.Categories).FirstOrDefaultAsync(u => u.UserId == userId);
        if (foundUser == null) return null;
        return new UserDto(
            foundUser.UserId,
            foundUser.Email,
            foundUser.Name,
            foundUser.Role,
            [..foundUser.Categories],
            foundUser.CreatedAt,
            foundUser.UpdatedAt
            );
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserDto user)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _db.Users
            .Where(u => u.UserId == user.UserId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Email, user.Email)
                .SetProperty(u => u.Name, user.Name)
                .SetProperty(u => u.RoleId, user.RoleId)
                .SetProperty(u => u.UpdatedAt, user.UpdatedAt));

        UserDto? updatedUser = _db.Users
            .Include(u => u.Role)
            .Include(u => u.Categories)
            .Select(u => new UserDto(
                u.UserId,
                u.Email,
                u.Name,
                u.Role,
                u.Categories.ToList<Category>(),
                u.CreatedAt,
                u.UpdatedAt
            ))
            .FirstOrDefault(u => u.UserId == user.UserId);

        return updatedUser;
    }
}
