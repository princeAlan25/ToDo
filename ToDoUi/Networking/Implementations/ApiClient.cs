using System.Net.Http.Json;
using ToDoUi.Networking.Interfaces;

namespace ToDoUi.Networking.Implementations;

public class ApiClient(HttpClient httpClient) : IApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task DeleteAsync<TRequest>(string endPoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(endPoint, cancellationToken);
        await Task.FromResult(response);
    }
    public async Task<TResponse?> GetAsync<TResponse>(string endPoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endPoint, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }
    public async Task<TResponse?> GetAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endPoint, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(endPoint, request, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(endPoint, request, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }
}
