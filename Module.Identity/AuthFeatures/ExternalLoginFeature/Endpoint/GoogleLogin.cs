using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.AuthFeatures.ExternalLoginFeature.Handler;
using Module.Identity.Extensions;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;

namespace Module.Identity.AuthFeatures.ExternalLoginFeature.Endpoint;

public class GoogleLogin : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.GoogleLogin, 
                async (GoogleSignInCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(GoogleLogin))
            .WithTags(IdentityEndpointConstants.Tags.Authentication)
            .Produces<Result<LoginResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary(nameof(GoogleLogin))
            .WithDescription("Sign in with Google");
    }
}