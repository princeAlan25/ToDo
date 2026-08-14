using ToDoUi.Networking.Interfaces;
using System.Net.Http.Headers;

namespace ToDoUi.Networking.Handlers;

public class AuthHandler(ITokenService tokenService): DelegatingHandler
{
    private readonly ITokenService _tokenService = tokenService;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? accessToken = _tokenService.GetAccessTokenAsync().Result;
        if(!string.IsNullOrWhiteSpace(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
