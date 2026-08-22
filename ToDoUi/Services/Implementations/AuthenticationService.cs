using ToDoShared.DTOs;
using ToDoUi.Networking.Interfaces;
using ToDoUi.Services.Interfaces;

namespace ToDoUi.Services.Implementations;

public class AuthenticationService(IApiClient apiClient, ITokenService tokenService) : IAuthenticationService
{
    private readonly IApiClient _apiClient = apiClient;
    public readonly ITokenService _tokenService = tokenService;
    public async Task<loginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        var response = await _apiClient.PostAsync<LoginRequestDto, loginResponseDto>("/auth/login", loginRequest);
        if(response is not null)
        {
            await _tokenService.SetAccessTokenAsync(response.AccessToken);
        }
        return response;
    }

    public async Task<UserDto?> SignUpAsync(SignUpRequestDto signUpRequest)
    {
        var response = await _apiClient.PostAsync<SignUpRequestDto, UserDto>("auth/signup", signUpRequest);
        return response;
    }

    public async Task<bool> LogOutAsync()
    {
        await _tokenService.RemoveAccessTokenAsync();
        return await _tokenService.HasAccessTokenAsync();
    }
}
