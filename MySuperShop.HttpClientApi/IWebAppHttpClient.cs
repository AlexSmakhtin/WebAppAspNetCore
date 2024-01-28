using MySuperShop.Domain.Entities;
using MySuperShop.HttpModels;

namespace MySuperShop.HttpClientApi
{
    public interface IWebAppHttpClient
    {
        Task<Product> GetProduct(Guid id, CancellationToken cts);
        Task<IReadOnlyList<Product>> GetProducts( CancellationToken cts);
        Task AddProduct(Product product, CancellationToken cts);
        Task UpdateProduct(Product product, CancellationToken cts);
        Task DeleteProduct(Guid id, CancellationToken cts);
        Task<RegistrationResponse> Register(RegistrationRequest request, CancellationToken cts);
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CancellationToken cts);
        Task<List<TrafficInfo>> GetTraffic(CancellationToken ct);

    }
}