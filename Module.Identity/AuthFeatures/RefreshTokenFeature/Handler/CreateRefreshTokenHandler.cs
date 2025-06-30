using Module.Identity.Persistence.JWT;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;
using Shared.DTOs.Common.Response;

namespace Module.Identity.AuthFeatures.RefreshTokenFeature.Handler;

public record CreateRefreshTokenCommand(string AccessToken,string RefreshToken) : ICommand<Result<RefreshTokenResponse>>;

public record RefreshTokenResponse (TokenDto AccessToken, TokenDto RefreshToken); 

public class CreateRefreshTokenHandler(ITokensService tokensService ) : ICommandHandler<CreateRefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var res = await  tokensService.RefreshAsync(request.AccessToken, request.RefreshToken);
        if (!res.HasValue)
            return ErrorCode.TokenCreationFailed;
        
        var accessToken = new TokenDto(res.Value.AccessToken, res.Value.AccessTokenExpiry);
        var refreshToken = new TokenDto(res.Value.RefreshToken, res.Value.RefreshTokenExpiry);
        return new RefreshTokenResponse(accessToken, refreshToken);
    }
}