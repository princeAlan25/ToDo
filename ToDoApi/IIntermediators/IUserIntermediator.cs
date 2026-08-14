using ToDoShared.DTOs;
using ToDoEntityModels.Models;

namespace ToDoApi.IIntermediators;

public interface IUserIntermediator
{
    IResult GetUserByIdAsync(string userId);
    IResult DeleteUserAsync(string userId);
    IResult UpdateUserAsync(UpdateUserDto user);
    IResult CreateUserAsync(SignUpRequestDto user);
}
