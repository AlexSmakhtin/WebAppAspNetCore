using Serilog;
using Backend.Controllers;
using Models;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories.Interfaces;
using Domain.Services;

namespace Backend
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Information("Server is up");
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog((ctx, conf) =>
                {
                    conf.MinimumLevel.Information()
                    .WriteTo.Console()
                    .MinimumLevel.Information();
                });
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                //builder.Services.AddDbContext<MyDbContext>(options =>
                //    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
                builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<AccountService>();
                builder.Services.AddCors();
                var app = builder.Build();
                app.UseCors(policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader();
                });
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
            }
            finally
            {
                Log.Information("Server shutted down");
                await Log.CloseAndFlushAsync();
            }
        }
    }
}