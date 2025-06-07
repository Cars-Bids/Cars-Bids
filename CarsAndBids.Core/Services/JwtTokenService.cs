using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CarsAndBids.Core.Services;

public class JwtTokenService(IConfiguration configuration,
                             UserManager<User> userManager) : IJwtTokenService
{
    public async Task<string> CreateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new("email", user.Email ?? ""),
            new("username", user.UserName ?? "")
        };
        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
            claims.Add(new("roles", role));

        var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSettings:SecretKey"));

        var signinKey = new SymmetricSecurityKey(key);

        var signinCredential = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            signingCredentials: signinCredential,
            expires: (DateTime.Now.AddDays(configuration.GetValue<int>("JwtSettings:AccessTokenExpiration"))).ToUniversalTime(),
            claims: claims);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public RefreshToken GenerateRefreshToken(User user)
    {
        var token = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JwtSettings:RefreshTokenExpiration")),
            Token = GenerateRefreshTokenOnly(),
            UserId = user.Id
        };
        
        return token;
    }

    public string GenerateRefreshTokenOnly()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}