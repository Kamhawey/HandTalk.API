using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Module.Identity.Persistence.Email;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Identity.PasswordFeatures.CreatePasswordResetTokenFeature.Handler;

public record CreatePasswordResetTokenCommand(string Email) : ICommand<Result>;

public class CreatePasswordResetTokenCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService)
    : ICommandHandler<CreatePasswordResetTokenCommand, Result>
{
    public async Task<Result> Handle(CreatePasswordResetTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure(ErrorCode.UnregisteredEmail);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        var subject = "Password Reset";
        await emailService.SendAsync(user.Email!, subject, token);
        
        return Result.Success();
    }
}