using System.IdentityModel.Tokens.Jwt;
using ToDoUi.Networking.Interfaces;

namespace ToDoUi.Networking.Implementations;

public class TokenService : ITokenService
{
    private const string AccessTokenKey = "access_token";
    public async Task<string?> GetAccessTokenAsync()
    {
        return await SecureStorage.GetAsync(AccessTokenKey);
    }

    public async Task<bool> HasAccessTokenAsync()
    {
        string? token = await SecureStorage.GetAsync(AccessTokenKey);
        return !string.IsNullOrWhiteSpace(token);
    }

    public bool IsTokenExpired(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenClaim = tokenHandler.ReadJwtToken(token);
        DateTime expirationTime = tokenClaim.ValidTo;
        return DateTime.UtcNow >= expirationTime;
    }

    public async Task RemoveAccessTokenAsync()
    {
        SecureStorage.Remove(AccessTokenKey);
    }

    public async Task SetAccessTokenAsync(string accessToken)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(accessToken, nameof(accessToken));
        await SecureStorage.SetAsync(AccessTokenKey, accessToken);
    }


}
