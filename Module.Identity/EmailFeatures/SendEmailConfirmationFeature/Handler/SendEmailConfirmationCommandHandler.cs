using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Module.Identity.Persistence.Email;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Identity.EmailFeatures.Handler;

public record SendEmailConfirmationCommand(string Email) : ICommand<Result>;

public class SendEmailConfirmationCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService)
    : ICommandHandler<SendEmailConfirmationCommand, Result>
{
    public async Task<Result> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure(ErrorCode.UnregisteredEmail);
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var subject = "Email Confirmation";
        await emailService.SendAsync(user.Email!, subject, token);
        
        return Result.Success();
    }
}