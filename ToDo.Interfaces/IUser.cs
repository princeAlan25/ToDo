using ToDoEntityModels.Models;

namespace ToDo.Interfaces;

public interface IUser
{
    public Task<User> CreateUserAsync(User user);
    public Task<User> UpdateUserAsync(User user);
    public Task<User?> GetUserById(Guid userId);
    public Task DeleteUserAsync(Guid userId);
}
