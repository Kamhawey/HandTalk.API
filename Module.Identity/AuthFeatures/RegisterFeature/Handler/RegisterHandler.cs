using Microsoft.AspNetCore.Identity;
using Module.Identity.Domain.Models;
using Module.Identity.Persistence.Email;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;

namespace Module.Identity.Features.RegisterFeature.Handler;

public record RegisterCommand(string FirstName , string LastName, string Email,string ProfilePictureUrl, string Password, string ConfirmPassword) : ICommand<Result>;


public class RegisterCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService)
    : ICommandHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Result.Failure(ErrorCode.EmailAlreadyExists);
        }

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            Email = request.Email,
            ProfilePictureUrl = request.ProfilePictureUrl,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result.Failure(ErrorCode.RegistrationFailed);
        }

        // Send confirmation email
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        // var confirmationLink = urlHelper.GenerateConfirmEmailUrl(user.Id, token);
        //
        // await emailService.SendAsync(user.Email!,"E-Mail Confirmation",);

        return Result.Success();
    }
}