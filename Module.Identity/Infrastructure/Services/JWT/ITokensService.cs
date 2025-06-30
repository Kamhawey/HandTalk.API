using Module.Identity.Domain.Models;

namespace Module.Identity.Persistence.JWT;

public interface ITokensService
{
   Task<RefreshToken?> SaveRefreshTokenAsync(
        long userId,
        string token,
        DateTimeOffset creationDate,
        DateTimeOffset expiryDate);

    Task<(long UserId, string AccessToken, DateTimeOffset AccessTokenExpiry, string RefreshToken, DateTimeOffset
            RefreshTokenExpiry)?> GenerateTokensAsync(ApplicationUser user);

    Task<bool> RemoveRefreshTokenAsync(string refreshToken); 
    Task<int> RemoveExpiredTokensAsync();

    Task<(long UserId, string AccessToken, DateTimeOffset AccessTokenExpiry, string RefreshToken, DateTimeOffset
        RefreshTokenExpiry)?> RefreshAsync(string accessToken, string refreshToken);
    
}