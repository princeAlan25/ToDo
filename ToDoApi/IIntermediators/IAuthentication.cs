using ToDoApi.DTOs;

namespace ToDoApi.IIntermediators;

public interface IAuthentication
{
    public IResult SignIn(LoginUserDto loginUserRequest);
    public IResult SignUp(CreateUserDto createUserRequest);
}
