using MySuperShop.Domain.Entities;
using MySuperShop.HttpModels;

namespace MySuperShop.HttpClientApi
{
    public interface IWebAppHttpClient
    {
        Task<Product> GetProduct(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Product>> GetProducts( CancellationToken ct);
        Task AddProduct(Product product, CancellationToken ct);
        Task UpdateProduct(Product product, CancellationToken ct);
        Task DeleteProduct(Guid id, CancellationToken ct);
        Task<RegistrationResponse> Register(RegistrationRequest request, CancellationToken ct);
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request, CancellationToken ct);
        Task<IReadOnlyCollection<TrafficInfo>> GetTraffic(CancellationToken ct);
        Task<Account> GetCurrentAccount(CancellationToken ct);
        Task SetHeader(string name, IEnumerable<string> values);
    }
}