using MySuperShop.Domain.Entities;
using MySuperShop.HttpModels;
using System.Net.Http.Json;
using MySuperShop.Domain.Exceptions;
using MySuperShop.HttpClientApi.Extensions;

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
                throw new MySuperShopException("Product is null");
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProducts(CancellationToken ct)
        {
            var uri = $"{_host}/api/products/get_all";
            var response = await _httpClient.GetFromJsonAsync<IReadOnlyList<Product>>(uri, cancellationToken: ct);
            if (response == null)
                throw new MySuperShopException("List products is null");
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
            var response =
                await _httpClient.PostAsJsonAsync<RegistrationRequest, RegistrationResponse>(uri, request, ct);
            if (response == null)
                throw new MySuperShopException("Registration response is null");
            return response;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request);
            var uri = $"{_host}/api/account/authentication";
            var response =
                await _httpClient.PostAsJsonAsync<AuthenticationRequest, AuthenticationResponse>(uri, request, ct);
            if (response == null)
                throw new MySuperShopException("Authentication response is null");
            return response;
        }

        public async Task<List<TrafficInfo>> GetTraffic(CancellationToken ct)
        {
            var uri = $"{_host}/api/traffic/get";
            using var response = await _httpClient.GetAsync(uri, cancellationToken: ct);
            response.EnsureSuccessStatusCode();
            var traffic =
                await response.Content.ReadFromJsonAsync<List<TrafficInfo>>(cancellationToken: ct);
            if (traffic == null)
                throw new MySuperShopException("Traffic list is null");
            return traffic;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}