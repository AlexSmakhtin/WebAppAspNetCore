using Frontend;
using MySuperShop.BlazorFrontend.Services.Implementations;
using MySuperShop.BlazorFrontend.Services.Interfaces;
using MySuperShop.HttpClientApi;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

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
            builder.Services.AddSingleton<IValidator, RegistrationRequestValidator>();
            Log.Logger = new LoggerConfiguration()
            .WriteTo.BrowserConsole()
            .CreateLogger();
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            await builder.Build().RunAsync();
        }
    }
}
