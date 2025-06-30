using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Extensions;
using Module.Identity.PasswordFeatures.CreatePasswordResetTokenFeature.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.PasswordFeatures.CreatePasswordResetTokenFeature.Endpoint;

public class CreatePasswordResetToken : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.CreatePasswordResetToken,
                async (CreatePasswordResetTokenCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(CreatePasswordResetToken))
            .WithTags(IdentityEndpointConstants.Tags.Password)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary(nameof(CreatePasswordResetToken))
            .WithDescription("Create password reset token and send to user's email");
    }
}