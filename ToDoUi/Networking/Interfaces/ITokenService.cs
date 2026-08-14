namespace ToDoUi.Networking.Interfaces;

public interface ITokenService
{
    Task<string?> GetAccessTokenAsync();
    Task SetAccessTokenAsync(string accessToken);
    Task RemoveAccessTokenAsync();
    Task<bool> HasAccessTokenAsync();
}
