using ToDoShared.DTOs;

namespace ToDoUi.Services.Interfaces;

public interface IAuthenticationService
{
    Task<loginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task<UserDto?> SignUpAsync(SignUpRequestDto signUpRequest);
}
