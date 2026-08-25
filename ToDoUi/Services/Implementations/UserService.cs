using System;
using System.Collections.Generic;
using System.Text;
using ToDoShared.DTOs;
using ToDoUi.Networking.Interfaces;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.Services.Implementations;

public class UserService(IApiClient apiClient) : IUserService
{
    private readonly IApiClient _apiClient = apiClient;
    public async Task<bool> DeleteUserAsync(string userId)
    {
        Dictionary<string, string> userQuery = new()
        {
            {"userId", userId }
        };
        return await _apiClient.DeleteAsync<Dictionary<string, string>, bool>("/user", userQuery);
    }

    public async Task<UserDto?> GetUserByIdAsync()
    {
        UserDto? userDto = await _apiClient.GetAsync<UserDto>("/user");
        return userDto;
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserDto user)
    {
        return await _apiClient.PutAsync<UpdateUserDto, UserDto>("/user", user);
    }
}
