using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Account;

public class LoginViaRefreshTokenQuery : IRequest<TokensDto>
{
    public string? RefreshToken { get; set; }
}

public class LoginViaRefreshTokenQueryHandler(
    IJwtTokenService jwtTokenService,
    IGenericRepository<RefreshToken> repository
    ) : IRequestHandler<LoginViaRefreshTokenQuery, TokensDto>
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
            throw new Exception(Resource.InvalidRefresh);
        }

        if (oldRefreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new Exception(Resource.RefreshExpired);
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