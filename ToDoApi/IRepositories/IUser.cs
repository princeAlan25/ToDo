using ToDoEntityModels.Models;

namespace ToDoApi.IRepositories;

public interface IUser
{
    public Task<User> CreateUserAsync(User user);
    public Task<User> UpdateUserAsync(User user);
    public Task<User?> GetUserById(Guid userId);
    public Task<bool> DeleteUserAsync(Guid userId);
}
