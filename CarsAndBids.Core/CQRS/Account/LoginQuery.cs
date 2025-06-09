using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.Core.CQRS.Account;

public class LoginQuery : IRequest<TokensDto>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class LoginQueryHandler(UserManager<User> userManager,
                             IJwtTokenService jwtTokenService,
                             IGenericRepository<RefreshToken> repository
                             ) : IRequestHandler<LoginQuery, TokensDto>
{
    public async Task<TokensDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(request.Email));

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(request.Password));

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new Exception($"Incorrect data");

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            throw new Exception($"Incorrect data");

        var accessToken = await jwtTokenService.CreateTokenAsync(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken(user);

        if (refreshToken.Token == null)
            throw new Exception("Generated refresh token is null.");

        await repository.InsertAsync(refreshToken);

        return new TokensDto() { AccessToken = accessToken, RefreshToken = refreshToken.Token };
    }
}
