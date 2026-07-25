using Microsoft.EntityFrameworkCore;
using ToDo.Interfaces;
using ToDoEntityModels.DataContexts;
using ToDoEntityModels.Models;

namespace ToDoApi.Services;

public class UserService(ToDoContext db, ILogger<UserService> logger) : IUser
{
    private readonly ToDoContext _db = db;
    public Task<User> CreateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        _db.Users.AddAsync(user);
        _db.SaveChangesAsync();
        return Task.FromResult(user);
    }

    public Task DeleteUserAsync(Guid userId)
    {
        if(userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        User userExist = _db.Users.First(u => u.UserId == userId);
        if (userExist == null)
        {
            logger.LogCritical($"{nameof(userExist)} does not exist");
        }
        else
        {
            _db.Users.Remove(userExist);
            _db.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        User? foundUser = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if(foundUser == null) logger.LogCritical($"user {userId} does not exist");
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
