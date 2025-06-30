using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.Infrastructure.Services;

namespace Module.Identity.ProfileFeatures.Handler;

public record EditProfileCommand(
    string FirstName,
    string LastName,
    string? ProfilePictureUrl
) : ICommand<Result>;

public class EditProfileCommandHandler(
    UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService)
    : ICommandHandler<EditProfileCommand, Result>
{
    public async Task<Result> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {

        var user = await userManager.FindByIdAsync(currentUserService.Id.ToString());
        if (user is null)
        {
            return Result.Failure(ErrorCode.UserNotFound);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        
        if (!string.IsNullOrWhiteSpace(request.ProfilePictureUrl))
           user.ProfilePictureUrl = request.ProfilePictureUrl;
        
        user.LastModifiedOn = DateTime.UtcNow;

        var result = await userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            return ErrorCode.FailedToUpdateData ;
        }
        
        return Result.Success();
    }
}