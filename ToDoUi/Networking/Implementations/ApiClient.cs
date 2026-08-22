using System.Net.Http.Json;
using ToDoUi.Networking.Interfaces;

namespace ToDoUi.Networking.Implementations;

public class ApiClient(HttpClient httpClient) : IApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="endPoint"></param>
    /// <param name="request">
    /// If you want to use queried endpoint, TRequest: Dictionary of string and object types
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse?> DeleteAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default)
    {
        string fullEndPoint = endPoint;
        if (request is Dictionary<string, object> queries)
        {
            foreach (string key in queries.Keys)
            {
                fullEndPoint += $"/{queries[key]}";
            }
        }
        var response = await _httpClient.DeleteAsync(fullEndPoint, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
    }

    public async Task<TResponse?> GetAsync<TResponse>(string endPoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endPoint, cancellationToken);
        if (!response.IsSuccessStatusCode || response.Content.Headers.ContentLength == 0) return default;
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
