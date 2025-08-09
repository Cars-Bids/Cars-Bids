using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using MediatR;

namespace CarsAndBids.Core.CQRS.Account;

public class LoginViaRefreshTokenQuery : IRequest<TokensDto>
{
    public string? RefreshToken { get; set; }
}

public class LoginViaRefreshTokenQueryHandler(IJwtTokenService jwtTokenService,
                                              IGenericRepository<RefreshToken> repository) : IRequestHandler<LoginViaRefreshTokenQuery, TokensDto>
{
    public async Task<TokensDto> Handle(LoginViaRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var refreshTokens = await repository.GetAsync(
            filter: token => token.Token == request.RefreshToken,
            includeProperties: "User"
        );

        var oldRefreshToken = refreshTokens.FirstOrDefault();
        if (oldRefreshToken == null || oldRefreshToken.User == null)
        {
            throw new Exception("Invalid refresh token or user not found");
        }

        if (oldRefreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new Exception("Refresh token has expired");
        }

        var accessToken = await jwtTokenService.CreateTokenAsync(oldRefreshToken.User);

        var newRefreshToken = jwtTokenService.GenerateRefreshToken(oldRefreshToken.User);

        await repository.InsertAsync(newRefreshToken);

        await repository.DeleteAsync(oldRefreshToken);

        return new TokensDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}