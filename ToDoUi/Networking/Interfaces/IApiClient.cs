using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoUi.Networking.Interfaces;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string endPoint, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync<TRequest>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
    Task<TResponse?> GetAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
}
