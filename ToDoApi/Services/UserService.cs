using ToDoApi.DTOs;
using ToDoApi.IIntermediators;
using ToDoApi.IRepositories;
using ToDoEntityModels.Models;

namespace ToDoApi.Services;

public class UserService(IUserRepository userRepo): IUserIntermediator
{
    private readonly IUserRepository _userRepo = userRepo;
    public IResult GetUserByIdAsync(string userId)
    {
        Guid id = Guid.Parse(userId);
        UserDto? user = _userRepo.GetUserById(id).Result;
        if (user == null) return Results.NotFound($"User {userId} not found");
        return Results.Ok(user);
    }

    public IResult DeleteUserAsync(string userId)
    {
        Guid id = Guid.Parse(userId);
        bool result = _userRepo.DeleteUserAsync(id).Result;
        if(result) return Results.Ok(result);
        return Results.NotFound($"User {userId} not found");
    }

    public IResult UpdateUserAsync(UpdateUserDto user)
    {
        UserDto? result = _userRepo.UpdateUserAsync(user).Result;
        if (result == null) return Results.Problem($"can not update {user.UserId}");
        return Results.Accepted(value: user);
    }

    public IResult CreateUserAsync(CreateUserDto user)
    {
        UserDto result = _userRepo.CreateUserAsync(user).Result;
        if (result == null) return Results.Problem($"Can not register user {user.Name}");
        return Results.Created<UserDto>("/auth/signup",value: result);
    }

}
