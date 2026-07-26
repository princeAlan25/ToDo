using ToDoEntityModels.Models;

namespace ToDoApi.IIntermediators;

public interface IUserIntermediator
{
    IResult GetUserByIdAsync(string userId);
    IResult DeleteUserAsync(string userId);
    IResult UpdateUserAsync(User user);
    IResult CreateUserAsync(User user);
}
