using MySuperShop.BlazorFrontend.Services.Implementations;
using MySuperShop.BlazorFrontend.Services.Interfaces;
using MySuperShop.HttpClientApi;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MySuperShop.BlazorFrontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped<IWebAppHttpClient, WebAppHttpClient>();
            builder.Services.AddSingleton<IValidator, RequestValidator>();
            
            await builder.Build().RunAsync();
        }
    }
}
