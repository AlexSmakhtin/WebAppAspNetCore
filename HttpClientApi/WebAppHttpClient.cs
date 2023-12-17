using Models;
using System.Net.Http.Json;

namespace HttpClientApi
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

        public async Task<Product> GetProduct(Guid id)
        {
            var uri = $"{_host}/api/products/get?id={id}";
            var product = await _httpClient.GetFromJsonAsync<Product>(uri);
            if (product == null)
                throw new NullReferenceException(nameof(product));
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProducts()
        {
            var uri = $"{_host}/api/products/get_all";
            var response = await _httpClient.GetFromJsonAsync<IReadOnlyList<Product>>(uri);
            if (response == null)
                throw new NullReferenceException(nameof(response));
            return response;
        }

        public async Task AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            var uri = $"{_host}/api/products/add";
            using var response = await _httpClient.PostAsJsonAsync(uri, product);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            var uri = $"{_host}/api/products/update";
            using var response = await _httpClient.PutAsJsonAsync(uri, product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Guid id)
        {
            var uri = $"{_host}/api/products/delete?id={id}";
            using var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
