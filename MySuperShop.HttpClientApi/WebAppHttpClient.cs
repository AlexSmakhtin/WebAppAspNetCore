using System.Collections.Concurrent;
using MySuperShop.Domain.Entities;
using MySuperShop.HttpModels;
using System.Net.Http.Json;

namespace MySuperShop.HttpClientApi
{
    public class WebAppHttpClient : IWebAppHttpClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string DefaultHost = "https://localhost:7028";
        private readonly string _host;

        public WebAppHttpClient(HttpClient? httpClient = null, string host = DefaultHost)
        {
            _host = host;
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<Product> GetProduct(Guid id, CancellationToken ct)
        {
            var uri = $"{_host}/api/products/get?id={id}";
            var product = await _httpClient.GetFromJsonAsync<Product>(uri, cancellationToken: ct);
            if (product == null)
                throw new NullReferenceException(nameof(product));
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProducts(CancellationToken ct)
        {
            var uri = $"{_host}/api/products/get_all";
            var response = await _httpClient.GetFromJsonAsync<IReadOnlyList<Product>>(uri, cancellationToken: ct);
            if (response == null)
                throw new NullReferenceException(nameof(response));
            return response;
        }

        public async Task AddProduct(Product product, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(product);
            var uri = $"{_host}/api/products/add";
            using var response = await _httpClient.PostAsJsonAsync(uri, product, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Product product, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(product);
            var uri = $"{_host}/api/products/update";
            using var response = await _httpClient.PutAsJsonAsync(uri, product, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Guid id, CancellationToken ct)
        {
            var uri = $"{_host}/api/products/delete?id={id}";
            using var response = await _httpClient.DeleteAsync(uri, ct);
            response.EnsureSuccessStatusCode();
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = $"{_host}/api/account/register";
            using var response = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            var registrationResponse =
                await response.Content.ReadFromJsonAsync<RegistrationResponse>(cancellationToken: ct);
            if (registrationResponse == null)
                throw new NullReferenceException(nameof(registrationResponse));
            return registrationResponse;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = $"{_host}/api/account/authentication";
            using var response = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            var authResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>(cancellationToken: ct);
            if (authResponse == null)
                throw new NullReferenceException(nameof(authResponse));
            return authResponse;
        }

        public async Task<Dictionary<string, int>> GetTraffic(CancellationToken ct)
        {
            var uri = $"{_host}/api/traffic/get";
            using var response = await _httpClient.GetAsync(uri, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            var traffic =
                await response.Content.ReadFromJsonAsync<Dictionary<string, int>>(cancellationToken: ct);
            if (traffic == null)
                throw new NullReferenceException(nameof(traffic));
            return traffic;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}