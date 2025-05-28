using CarsAndBids.API.Services;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using CarsAndBids.Data.Persistence.Repositories;
using CarsAndBids.Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CarsAndBids.Core.Services;

namespace CarsAndBids.API.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(CarsAndBids.Core.Mapping.AutoMapperProfile));

            services.AddScoped<IGenericRepository<Auction>, GenericRepository<Auction>>();
            services.AddScoped<IAuctionService, AuctionService>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // var jwtOpts = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;
            // services.AddSingleton(_ => jwtOpts!);
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(o =>
            //     {
            //         o.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = false,
            //             ValidateAudience = false,
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = jwtOpts.Issuer,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key)),
            //             ClockSkew = TimeSpan.Zero
            //         };
            //     });

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });

            services.AddSingleton<IWebHostEnvironment>(environment);
            // services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var folderName = config.GetValue<string>("FileStorage:FolderName") ?? "Files";
                var maxFileSize = config.GetValue<long>("FileStorage:MaxFileSize", 5 * 1024 * 1024);
                return new FileService(env.WebRootPath, folderName, maxFileSize);
            });


            return services;
        }
    }
}