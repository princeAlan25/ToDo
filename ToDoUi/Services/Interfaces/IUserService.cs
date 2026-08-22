using System;
using System.Collections.Generic;
using System.Text;
using ToDoShared.DTOs;

namespace ToDoUi.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync();
    Task<bool> DeleteUserAsync(string userId);
    Task<UserDto?> UpdateUserAsync(UpdateUserDto user);
}
