using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.Infrastructure.Services;

namespace Module.Identity.ProfileFeatures.Handler;

public record GetCurrentUserQuery() : IQuery<Result<UserProfileResponse>>;

public record UserProfileResponse(
    long Id,
    string Email,
    string FirstName,
    string LastName,
    string? ProfilePictureUrl,
    bool IsActive,
    DateTime CreatedOn,
    DateTime? LastModifiedOn
);

public class GetCurrentUserQueryHandler(
    UserManager<ApplicationUser> userManager,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetCurrentUserQuery, Result<UserProfileResponse>>
{
    public async Task<Result<UserProfileResponse>> Handle(GetCurrentUserQuery request , CancellationToken cancellationToken)
    {
        
        var user = await userManager.FindByIdAsync(currentUserService.Id.ToString()!);
        if (user is null)
        {
            return ErrorCode.UserNotFound;
        }

        var response = new UserProfileResponse(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            user.ProfilePictureUrl,
            user.IsActive,
            user.CreatedOn,
            user.LastModifiedOn
        );

        return Result<UserProfileResponse>.Success(response);
    }
}
