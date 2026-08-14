using System.Text.RegularExpressions;
using ToDoShared.DTOs;

namespace ToDoApi.Utilities;

public static class Validator
{
    public static string? ValidateUserSignUp(SignUpRequestDto createUserRequest)
    {
        if (createUserRequest == null) return "Invalid User Credentials";
        if (string.IsNullOrWhiteSpace(createUserRequest.Email) || !Regex.IsMatch(createUserRequest.Email,"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")) return "Invalid Email";
        if (string.IsNullOrWhiteSpace(createUserRequest.Password)) return "Invalid Password";
        if (string.IsNullOrWhiteSpace(createUserRequest.Name)) return "Invalid UserName";
        return null;
    }

    public static string? ValidateUserSignIn(LoginRequestDto loginUserRequest)
    {
        if (loginUserRequest == null) return "Invalid User credentials";
        if (string.IsNullOrWhiteSpace(loginUserRequest.Email) || !Regex.IsMatch(loginUserRequest.Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")) return "Invalid Email"; ;
        if (string.IsNullOrWhiteSpace(loginUserRequest.Password)) return "Invalid Password";
        return null;
    }
}
