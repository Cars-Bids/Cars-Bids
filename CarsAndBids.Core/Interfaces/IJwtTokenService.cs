using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(User user);
    RefreshToken GenerateRefreshToken(User user);
    string GenerateRefreshTokenOnly();
}