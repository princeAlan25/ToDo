using ToDoApi.DTOs;

namespace ToDoApi.Utilities;

public static class Validator
{
    public static IResult ValidateUserSignUp(CreateUserDto createUserRequest)
    {
        if (createUserRequest == null) return Results.BadRequest("User credentials are required");
        if (string.IsNullOrWhiteSpace(createUserRequest.Email)) return Results.BadRequest("Email is required");
        if (string.IsNullOrWhiteSpace(createUserRequest.Password)) return Results.BadRequest("Password is required");
        if (string.IsNullOrWhiteSpace(createUserRequest.Name)) return Results.BadRequest("UserName is required");
        return Results.Empty;
    }

    public static IResult ValidateUserSignIn(LoginUserDto loginUserRequest)
    {
        if (loginUserRequest == null) return Results.BadRequest("User credentials are required");
        if (string.IsNullOrWhiteSpace(loginUserRequest.email)) return Results.BadRequest("Email is required");
        if (string.IsNullOrWhiteSpace(loginUserRequest.password)) return Results.BadRequest("Password is required");
        return Results.Empty;
    }
}
