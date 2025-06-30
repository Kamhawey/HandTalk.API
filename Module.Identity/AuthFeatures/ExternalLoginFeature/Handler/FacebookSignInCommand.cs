using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Module.Identity.AuthFeatures.ExternalLoginFeature.Config;
using Module.Identity.Domain.Models;
using Module.Identity.Persistence.JWT;
using Newtonsoft.Json;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;
using Shared.DTOs.Common.Response;

namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Handler;

#region Facebook Response

public class FacebookTokenValidationResponse
{
    [JsonProperty("data")]
    public FacebookTokenValidationData Data { get; set; }
}
public class FacebookTokenValidationData
{
    [JsonProperty("app_id")]
    public string AppId { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("application")]
    public string Application { get; set; }

    [JsonProperty("data_access_expires_at")]
    public long DataAccessExpiresAt { get; set; }

    [JsonProperty("expires_at")]
    public long ExpiresAt { get; set; }

    [JsonProperty("is_valid")]
    public bool IsValid { get; set; }

    [JsonProperty("metadata")]
    public Metadata Metadata { get; set; }

    [JsonProperty("scopes")]
    public string[] Scopes { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }
}

public class Metadata
{
    [JsonProperty("auth_type")]
    public string AuthType { get; set; }
}

public class FacebookUserInfoResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("picture")]
    public Picture Picture { get; set; }
}

public class Picture
{
    [JsonProperty("data")]
    public Data Data { get; set; }
}

public class Data
{
    [JsonProperty("height")]
    public long Height { get; set; }

    [JsonProperty("is_silhouette")]
    public bool IsSilhouette { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("width")]
    public long Width { get; set; }
}
#endregion
public record FacebookSignInCommand(string AccessToken) : ICommand<Result<LoginResponseDto>>;
public class FacebookSignInCommandHandler(
    UserManager<ApplicationUser> userManager,
    IOptions<FacebookAuthConfig> facebookAuthConfig,
    IHttpClientFactory httpClientFactory,
    ITokensService tokensService,
    ILogger<FacebookSignInCommandHandler> logger)
    : ICommandHandler<FacebookSignInCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(FacebookSignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient("Facebook");
            var config = facebookAuthConfig.Value;

            var tokenValidationUrl = string.Format(
                config.TokenValidationUrl, 
                request.AccessToken, 
                config.AppId, 
                config.AppSecret);
                
            var tokenValidationResponse = await httpClient.GetFromJsonAsync<FacebookTokenValidationResponse>(
                tokenValidationUrl, cancellationToken);

            if (tokenValidationResponse?.Data is null || !tokenValidationResponse.Data.IsValid)
            {
                logger.LogWarning("Invalid Facebook token");
                return Result.Failure<LoginResponseDto>(ErrorCode.InvalidToken);
            }

            var userInfoUrl = string.Format(config.UserInfoUrl, request.AccessToken);
            var userInfoResponse = await httpClient.GetFromJsonAsync<FacebookUserInfoResponse>(
                userInfoUrl, cancellationToken);

            if (userInfoResponse is null || string.IsNullOrEmpty(userInfoResponse.Email))
            {
                logger.LogWarning("Failed to retrieve user information from Facebook");
                return Result.Failure<LoginResponseDto>(ErrorCode.ExternalLoginFailed);
            }

            var user = await userManager.FindByEmailAsync(userInfoResponse.Email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = userInfoResponse.Email,
                    UserName = userInfoResponse.Email,
                    FirstName = userInfoResponse.FirstName,
                    LastName = userInfoResponse.LastName,
                    EmailConfirmed = true,
                    ProfilePictureUrl = userInfoResponse.Picture?.Data?.Url?.ToString()
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    logger.LogError("Failed to create user from Facebook login: {Errors}", 
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return Result.Failure<LoginResponseDto>(ErrorCode.ExternalLoginFailed);
                }

                var addLoginResult = await userManager.AddLoginAsync(
                    user, 
                    new UserLoginInfo("Facebook", userInfoResponse.Id, "Facebook"));
                    
                if (!addLoginResult.Succeeded)
                {
                    logger.LogError("Failed to add external login for user: {Errors}", 
                        string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
                    return Result.Failure<LoginResponseDto>(ErrorCode.ExternalLoginFailed);
                }
            }

            var tokens = await tokensService.GenerateTokensAsync(user);
            if (tokens == null)
                return Result.Failure<LoginResponseDto>(ErrorCode.TokenCreationFailed);

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                Token = tokens.Value.AccessToken,
                RefreshToken = tokens.Value.RefreshToken,
                CreatedOn = user.CreatedOn,
                TokenExpiry = tokens.Value.AccessTokenExpiry,
                RefreshTokenExpiry = tokens.Value.RefreshTokenExpiry
            };

            return Result.Success(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Facebook API request failed");
            return Result.Failure<LoginResponseDto>(ErrorCode.ExternalLoginFailed);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Facebook authentication failed");
            return Result.Failure<LoginResponseDto>(ErrorCode.ExternalLoginFailed);
        }
    }
}