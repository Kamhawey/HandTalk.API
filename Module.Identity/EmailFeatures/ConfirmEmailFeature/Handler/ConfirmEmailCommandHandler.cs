using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Identity.EmailFeatures.ConfirmEmailFeature.Handler;

public record ConfirmEmailCommand(string Email, string VerificationCode) : ICommand<Result>;

public class ConfirmEmailCommandHandler(
    UserManager<ApplicationUser> userManager)
    : ICommandHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure(ErrorCode.UnregisteredEmail);
        }

        var result = await userManager.ConfirmEmailAsync(user, request.VerificationCode);
        
        if (!result.Succeeded)
        {
            return Result.Failure(ErrorCode.VerificationCodeNotValid);
        }
        
        return Result.Success();
    }
}