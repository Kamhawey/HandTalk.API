using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Extensions;
using Module.Identity.PasswordFeatures.ResetPassword.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.PasswordFeatures.ResetPasswordFeature.Endpoint;

public class ResetPassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.ResetPassword,
                async (ResetPasswordCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(ResetPassword))
            .WithTags(IdentityEndpointConstants.Tags.Password)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary(nameof(ResetPassword))
            .WithDescription("Reset user password with token");
    }
}