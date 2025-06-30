using Shared.Core.CQRS;
using Shared.DTOs.Common;

namespace Module.Identity.PasswordFeatures.ChangePasswordFeature.Handler;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : ICommand<Result>;

// public class ChangePasswordCommandHandler(
//     UserManager<ApplicationUser> userManager,
//     ICurrentUserService currentUser)
//     : ICommandHandler<ChangePasswordCommand, Result>
// {
//     public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
//     {
//         var user = await userManager.FindByIdAsync(currentUser.UserId.ToString());
//         if (user is null)
//         {
//             return Result.Failure(ErrorCode.Unauthorized);
//         }
//
//         var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
//         
//         if (!result.Succeeded)
//         {
//             return Result.Failure(ErrorCode.ValidationFailed, 
//                 result.Errors.Select(e => e.Description).ToArray());
//         }
//         
//         return Result.Success();
//     }
// }