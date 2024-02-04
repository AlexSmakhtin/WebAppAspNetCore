using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using MySuperShop.ApiGateway.DbContexts;
using MySuperShop.ApiGateway.Services;
using MySuperShop.Domain.Entities;
using MySuperShop.Domain.Services;
using MySuperShop.ApiGateway.Middleware;
using MySuperShop.ApiGateway.Repositories;

namespace MySuperShop.ApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Information("Server is up");
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog((hbc, conf) =>
                {
                    conf.MinimumLevel.Information()
                        .WriteTo.Console()
                        .MinimumLevel.Information();
                });
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddDbContext<MyDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
                builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<ITrafficRepository, TrafficRepository>();
                builder.Services.AddScoped<AccountService>();
                builder.Services.AddSingleton<IPasswordHasher<Account>, PasswordHasher<Account>>();
                builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
                builder.Services.AddSingleton<ITrafficMeasurementService, TrafficMeasurementService>();
                builder.Services.AddCors();
                builder.Services.AddHttpLogging(options =>
                {
                    options.LoggingFields = HttpLoggingFields.RequestHeaders
                                            | HttpLoggingFields.ResponseHeaders
                                            | HttpLoggingFields.RequestBody
                                            | HttpLoggingFields.ResponseBody;
                });
                var app = builder.Build();
                app.UseHttpLogging();
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
                //app.UseMiddleware<OnlyEdgeMiddleware>();
                app.UseMiddleware<TrafficCounterMiddleware>();
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
            }
            finally
            {
                Log.Information("Server shutting down");
                await Log.CloseAndFlushAsync();
            }
        }
    }
}