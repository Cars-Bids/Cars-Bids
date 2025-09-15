using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Steria.API.Filters;
using Steria.API.HostedServices;
using Steria.API.Hubs;
using Steria.API.Middleware;
using Steria.API.Notifiers;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Mapping;
using Steria.Data.Persistence;
using Steria.Data.Persistence.Repositories;
using Steria.Data.Persistence.Seed;
using Steria.Data.Services;
using System.Text;
using ZiggyCreatures.Caching.Fusion;

namespace Steria.API.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")),
            lifetime: ServiceLifetime.Scoped
        );

        services.AddScoped<DataSeeder>();


        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddFusionCache()
                .WithDefaultEntryOptions(new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(30),
                    FailSafeMaxDuration = TimeSpan.FromHours(1),
                    JitterMaxDuration = TimeSpan.FromSeconds(30)
                });
      
        services.AddScoped<IDataSeederRepository, DataSeederRepository>();
        services.AddScoped<IAuctionRepository, AuctionRepository>();

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
        services.AddScoped<IGenericRepository<UserChatMessageReaction>, GenericRepository<UserChatMessageReaction>>();
        services.AddScoped<IGenericRepository<EmojiReaction>, GenericRepository<EmojiReaction>>();
        services.AddScoped<IGenericRepository<Wishlist>, GenericRepository<Wishlist>>();
        services.AddScoped<IGenericRepository<Bid>, GenericRepository<Bid>>();
        services.AddScoped<IGenericRepository<Comment>, GenericRepository<Comment>>();
        services.AddScoped<IGenericRepository<NotificationType>, GenericRepository<NotificationType>>();
        services.AddScoped<IGenericRepository<UserNotificationSetting>, GenericRepository<UserNotificationSetting>>();
        services.AddScoped<IGenericRepository<UserNotification>, GenericRepository<UserNotification>>();
        services.AddScoped<IGenericRepository<UserFollow>, GenericRepository<UserFollow>>();
        services.AddScoped<IGenericRepository<Answer>, GenericRepository<Answer>>();
        
        services.AddSingleton<IConnectionManager<ChatHub>, ConnectionManager<ChatHub>>();
        services.AddSingleton<IConnectionManager<NotificationHub>, ConnectionManager<NotificationHub>>();

        services.AddScoped<IRealtimeNotifier, SignalRNotifier>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddSingleton<IEmailQueue, EmailQueue>();
        services.AddHostedService<EmailBackgroundService>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); //SignalR
            });
        });

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