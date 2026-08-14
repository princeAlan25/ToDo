using ToDoShared.DTOs;

namespace ToDoApi.IIntermediators;

public interface IAuthentication
{
    public IResult SignIn(LoginRequestDto loginUserRequest);
    public IResult SignUp(SignUpRequestDto createUserRequest);
}
