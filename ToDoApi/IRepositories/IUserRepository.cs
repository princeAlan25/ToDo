using ToDoApi.DTOs;
using ToDoEntityModels.Models;

namespace ToDoApi.IRepositories;

public interface IUserRepository
{
    public Task<UserDto> CreateUserAsync(User user);
    public Task<UserDto> UpdateUserAsync(User user);
    public Task<UserDto?> GetUserById(Guid userId);
    public Task<bool> DeleteUserAsync(Guid userId);
    public Task<User?> GetUserByEmailAsync(string email);
}
