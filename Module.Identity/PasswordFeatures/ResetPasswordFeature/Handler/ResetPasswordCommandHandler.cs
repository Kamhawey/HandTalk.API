using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Identity.PasswordFeatures.ResetPassword.Handler;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : ICommand<Result>;

public class ResetPasswordCommandHandler(
    UserManager<ApplicationUser> userManager)
    : ICommandHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure(ErrorCode.UnregisteredEmail);
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        
        if (!result.Succeeded)
        {
            return Result.Failure(ErrorCode.VerificationCodeNotValid);
        }
        
        return Result.Success();
    }
}