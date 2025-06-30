using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Extensions;
using Module.Identity.PasswordFeatures.ChangePasswordFeature.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.PasswordFeatures.ChangePasswordFeature.Endpoint;

public class ChangePassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.ChangePassword,
                async (ChangePasswordCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(ChangePassword))
            .WithTags(IdentityEndpointConstants.Tags.Password)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary(nameof(ChangePassword))
            .WithDescription("Change user password")
            .RequireAuthorization();

    }
}