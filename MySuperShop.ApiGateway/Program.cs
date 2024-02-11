using ApiGateway.Services.Implementations;
using ApiGateway.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Microsoft.EntityFrameworkCore;
using MySuperShop.Domain.Repositories.Interfaces;
using MySuperShop.Domain.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySuperShop.ApiGateway.Configs;
using MySuperShop.ApiGateway.DbContexts;
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
                var jwtConfig = builder.Configuration
                    .GetRequiredSection("JwtConfig")
                    .Get<JwtConfig>();
                if (jwtConfig is null)
                {
                    throw new InvalidOperationException("JwtConfig is not configured");
                }

                builder.Services.AddOptions<JwtConfig>()
                    .Bind(builder.Configuration.GetSection("JwtConfig"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                builder.Services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            RequireExpirationTime = true,
                            RequireSignedTokens = true,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidAudiences = new[] { jwtConfig.Audience },
                            ValidIssuer = jwtConfig.Issuer
                        };
                    });
                builder.Services.AddAuthorization();
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. " +
                                      "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                                      "\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });
                builder.Services.AddDbContext<MyDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
                builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<ITrafficRepository, TrafficRepository>();
                builder.Services.AddScoped<AccountService>();
                builder.Services.AddSingleton<IPasswordHasher<Account>, PasswordHasher<Account>>();
                builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
                builder.Services.AddSingleton<ITrafficMeasurementService, TrafficMeasurementService>();
                builder.Services.AddSingleton<JwtService>();
                builder.Services.AddCors();
                builder.Services.AddHttpLogging(options =>
                {
                    options.LoggingFields = HttpLoggingFields.RequestHeaders
                                            | HttpLoggingFields.ResponseHeaders
                                            | HttpLoggingFields.RequestBody
                                            | HttpLoggingFields.ResponseBody;
                });

                var app = builder.Build();
                app.UseCors(policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .WithOrigins("https://localhost:7145","https://localhost:5075")
                        .AllowAnyHeader();
                });
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
                //app.UseMiddleware<OnlyEdgeMiddleware>();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseHttpLogging();
                app.UseMiddleware<TrafficCounterMiddleware>();
                app.MapControllers();
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