using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Module.Identity.Persistence.JWT;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;
using Shared.DTOs.Common.Response;

namespace Module.Identity.AuthFeatures.LoginFeature.Handler;


public record LoginCommand(string Email, string Password, bool RememberMe) : ICommand<Result<LoginResponseDto>>;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokensService tokensService)
    : ICommandHandler<LoginCommand, Result<LoginResponseDto>>
{

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.Failure<LoginResponseDto>(ErrorCode.InvalidCredentials);
        }
        if (!user.EmailConfirmed)
        {
            return Result.Failure<LoginResponseDto>(ErrorCode.EmailNotConfirmed);
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, request.RememberMe);
        if (!result.Succeeded)
        {
            return Result.Failure<LoginResponseDto>(ErrorCode.InvalidCredentials);
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
            RefreshTokenExpiry =  tokens.Value.RefreshTokenExpiry
        };

        return Result.Success(response);
    }
}