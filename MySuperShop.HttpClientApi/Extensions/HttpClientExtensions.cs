using System.Net;
using System.Net.Http.Json;
using MySuperShop.Domain.Exceptions;

namespace MySuperShop.HttpClientApi.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> PostAsJsonAsync<TRequest, TResponse>(
        this HttpClient client, string? requestUri, TRequest request, CancellationToken ct)
    {
        using var response = await client.PostAsJsonAsync(requestUri, request, cancellationToken: ct);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: ct);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var details = await response.Content.ReadAsStringAsync(cancellationToken: ct);
            throw new MySuperShopException(response.StatusCode, details);
        }

        var message = await response.Content.ReadAsStringAsync(ct);
        throw new MySuperShopException(response.StatusCode,
            string.IsNullOrWhiteSpace(message) ? "Unknown Error" : message);
    }
}