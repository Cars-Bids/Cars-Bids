using Steria.Core.Entities;

namespace Steria.Core.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(User user);
    RefreshToken GenerateRefreshToken(User user);
    string GenerateRefreshTokenOnly();
}