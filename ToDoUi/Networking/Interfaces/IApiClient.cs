using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoUi.Networking.Interfaces;

public interface IApiClient
{
    Task<TResponse?> GetAsync<TResponse>(string endPoint, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
    Task<TResponse?> DeleteAsync<TRequest, TResponse>(string endPoint, TRequest request, CancellationToken cancellationToken = default);
}
