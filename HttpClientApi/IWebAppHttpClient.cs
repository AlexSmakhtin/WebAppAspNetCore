﻿using Domain.Entities;
using HttpModels;

namespace HttpClientApi
{
    public interface IWebAppHttpClient
    {
        Task<Product> GetProduct(Guid id);
        Task<IReadOnlyList<Product>> GetProducts();
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Guid id);
        Task<Account> Register(RegistrationRequest _request);
    }
}