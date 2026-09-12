using ToDoShared.DTOs;

namespace ToDoUi.Services.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task<UserDto?> SignUpAsync(SignUpRequestDto signUpRequest);

    Task<bool> LogOutAsync();
}
