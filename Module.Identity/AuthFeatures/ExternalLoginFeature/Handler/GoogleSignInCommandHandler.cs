using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Module.Identity.AuthFeatures.ExternalLoginFeature.Config;
using Module.Identity.Domain.Models;
using Module.Identity.Persistence.JWT;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;
using Shared.DTOs.Common.Response;

namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Handler;


public record GoogleSignInCommand(string IdToken) : ICommand<Result<LoginResponseDto>>;

public class GoogleSignInCommandHandler(
    UserManager<ApplicationUser> userManager,
    IOptions<GoogleAuthConfig> googleAuthConfig,
    ITokensService tokensService,
    ILogger<GoogleSignInCommandHandler> logger)
    : ICommandHandler<GoogleSignInCommand, Result<LoginResponseDto>>
{
    private readonly GoogleAuthConfig _googleAuthConfig = googleAuthConfig.Value;

    public async Task<Result<LoginResponseDto>> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_googleAuthConfig.ClientId]
            });

            var user = await userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    EmailConfirmed = true,
                    ProfilePictureUrl = payload.Picture
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    logger.LogError("Failed to create user from Google login: {Errors}", 
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return ErrorCode.ExternalLoginFailed;
                }

                var addLoginResult = await userManager.AddLoginAsync(user, new UserLoginInfo("Google", payload.Subject, "Google"));
                if (!addLoginResult.Succeeded)
                {
                    logger.LogError("Failed to add external login for user: {Errors}", 
                        string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
                    return ErrorCode.ExternalLoginFailed;
                }
            }
            var tokens = await tokensService.GenerateTokensAsync(user);
            if (tokens == null)
                return Result.Failure<LoginResponseDto>(ErrorCode.TokenCreationFailed);

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Token = tokens.Value.AccessToken,
                RefreshToken = tokens.Value.RefreshToken,
                CreatedOn = user.CreatedOn,
                TokenExpiry = tokens.Value.AccessTokenExpiry,
                RefreshTokenExpiry = tokens.Value.RefreshTokenExpiry
            };

            return Result.Success(response);
        }
        catch (InvalidJwtException ex)
        {
            logger.LogError(ex, "Invalid Google token");
            return Result.Failure<LoginResponseDto>(ErrorCode.InvalidToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Google authentication failed");
            return Result.Failure<LoginResponseDto>(ErrorCode.ExternalLoginFailed);
        }
    }
}