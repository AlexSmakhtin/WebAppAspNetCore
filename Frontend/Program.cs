using HttpClientApi;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;

namespace Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped<IWebAppHttpClient, WebAppHttpClient>();
            Log.Logger = new LoggerConfiguration()
            .WriteTo.BrowserConsole()
            .CreateLogger();

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            await builder.Build().RunAsync();
        }
    }
}
