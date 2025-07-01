using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.Infrastructure.Services;

namespace Module.Identity.PasswordFeatures.ChangePasswordFeature.Handler;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : ICommand<Result>;

public class ChangePasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUser)
    : ICommandHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(currentUser.Id.ToString());
        if (user is null)
        {
            return ErrorCode.NotFound;
        }

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return ErrorCode.ValidationError;
        }

        return Result.Success();
    }
}