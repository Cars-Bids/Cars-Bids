using CarsAndBids.API.Filters;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Services;
using CarsAndBids.Data.Persistence;
using CarsAndBids.Data.Persistence.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using CarsAndBids.Data.Persistence.Seed;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.DTOs;

namespace CarsAndBids.API.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<DataSeeder>();


        services.AddAutoMapper(typeof(Core.Mapping.AutoMapperProfile));
      
        services.AddScoped<IDataSeederRepository, DataSeederRepository>();
      
        services.AddScoped<IGenericRepository<Auction>, GenericRepository<Auction>>();
        services.AddScoped<IGenericRepository<RefreshToken>, GenericRepository<RefreshToken>>();
        services.AddScoped<IGenericRepository<BodyStyle>, GenericRepository<BodyStyle>>();
        services.AddScoped<IGenericRepository<Make>, GenericRepository<Make>>();
        services.AddScoped<IGenericRepository<Model>, GenericRepository<Model>>();
        services.AddScoped<IGenericRepository<CarImage>, GenericRepository<CarImage>>();
        services.AddScoped<IGenericRepository<Car>, GenericRepository<Car>>();
        services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
        services.AddScoped<IGenericRepository<Chat>, GenericRepository<Chat>>();
        services.AddScoped<IGenericRepository<Wishlist>, GenericRepository<Wishlist>>();
        services.AddScoped<IGenericRepository<ChatMessage>, GenericRepository<ChatMessage>>();
        services.AddScoped<IGenericRepository<ChatAttachment>, GenericRepository<ChatAttachment>>();

        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddSignalR();
        services.AddHostedService<AuctionHostedService>();

        services.AddControllers();

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMvc(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpContextAccessor();

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                configuration["JwtSettings:SecretKey"]
                ?? throw new NullReferenceException("JwtSettings:SecretKey")
            )
        );

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = "nameid"
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/hub"))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "Jwt Auth header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                }
            );
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
        
        services.AddIdentity<User, IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddSingleton<IWebHostEnvironment>(environment);
        services.AddSingleton<IUserIdProvider, UserIdProvider>();


        return services;
    }
}