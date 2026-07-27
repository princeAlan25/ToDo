using Microsoft.EntityFrameworkCore;
using ToDoApi.IRepositories;
using ToDoApi.Utilities;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Repositories;

public class UserRepo(ToDoContext db, ILogger<UserRepo> logger) : IUser
{
    private readonly ToDoContext _db = db;
    public Task<User> CreateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.Password = Securit.HashPassword(user.Password);
        _db.Users.AddAsync(user);
        _db.SaveChangesAsync();
        return Task.FromResult(user);
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

    public async Task<User?> GetUserById(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        User? foundUser = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return foundUser ?? null;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        try
        {
            _db.Entry<User>(user).CurrentValues.SetValues(user);
            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            logger.LogError($"{nameof(user)} does not exist or operation failed");
        }
        return user;
    }
}
