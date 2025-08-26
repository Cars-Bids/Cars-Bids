using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Account;

public class LoginQuery : IRequest<TokensDto>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginQueryHandler(
    UserManager<User> userManager,
    IJwtTokenService jwtTokenService,
    IGenericRepository<RefreshToken> repository
    ) : IRequestHandler<LoginQuery, TokensDto>
{
    public async Task<TokensDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new HttpException(Resource.IncorectData, HttpStatusCode.Unauthorized);

        if (!await userManager.CheckPasswordAsync(user, request.Password))
            throw new HttpException(Resource.IncorectData, HttpStatusCode.Unauthorized);

        var accessToken = await jwtTokenService.CreateTokenAsync(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken(user);

        if (refreshToken.Token is null)
            throw new Exception("Generated refresh token is null");

        await repository.InsertAsync(refreshToken);

        return new TokensDto 
        { 
            AccessToken = accessToken, 
            RefreshToken = refreshToken.Token 
        };
    }
}
