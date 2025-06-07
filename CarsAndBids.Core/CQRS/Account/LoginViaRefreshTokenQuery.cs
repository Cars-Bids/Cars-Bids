using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Services;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Account;

public class LoginViaRefreshTokenQuery : IRequest<TokensDto>
{
    public string RefreshToken { get; set; }
}

public class LoginViaRefreshTokenQueryHandler(IJwtTokenService jwtTokenService,
                                              IGenericRepository<RefreshToken> repository) : IRequestHandler<LoginViaRefreshTokenQuery, TokensDto>
{
    public async Task<TokensDto> Handle(LoginViaRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        // Отримання токенів з бази за допомогою фільтра та включення користувача
        var refreshTokens = await repository.GetAsync(
            filter: token => token.Token == request.RefreshToken,
            includeProperties: "User" // Включаємо навігаційну властивість User
        );

        // Беремо перший токен
        var oldRefreshToken = refreshTokens.FirstOrDefault();
        if (oldRefreshToken == null)
        {
            throw new Exception("Invalid refresh token");
        }

        // Перевірка, чи токен не прострочений
        if (oldRefreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            throw new Exception("Refresh token has expired");
        }

        // Генерація нового access-токена на основі даних користувача
        var accessToken = await jwtTokenService.CreateTokenAsync(oldRefreshToken.User);
        
        // Створення нового рефреш-токена
        var newRefreshToken = jwtTokenService.GenerateRefreshToken(oldRefreshToken.User);

        // Додавання нового токена в базу
        await repository.InsertAsync(newRefreshToken);

        // Видалення старого токена
        await repository.DeleteAsync(oldRefreshToken);
        
        // Повернення результату
        return new TokensDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}