using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using ToDoUi.Networking.Interfaces;
using System.Net;

namespace ToDoUi.Networking.Handlers;

public class AuthHandler(ITokenService tokenService): DelegatingHandler
{
    private readonly ITokenService _tokenService = tokenService;
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? accessToken = _tokenService.GetAccessTokenAsync().Result;
        if(!string.IsNullOrWhiteSpace(accessToken))
        {
            if(_tokenService.IsTokenExpired(accessToken))
            {
                await _tokenService.RemoveAccessTokenAsync();
            }
            else
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        HttpResponseMessage? response = await base.SendAsync(request, cancellationToken);

        //check authorization status for local token logical reference
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _tokenService.RemoveAccessTokenAsync();
        }
        return response;
    }
}
