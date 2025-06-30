using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.AuthFeatures.RefreshTokenFeature.Handler;
using Module.Identity.Extensions;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;

namespace Module.Identity.AuthFeatures.RefreshTokenFeature;

public class CreateRefreshToken : ICarterModule
    
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.RefreshToken,
                async (CreateRefreshTokenCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(CreateRefreshToken))
            .WithTags(IdentityEndpointConstants.Tags.Authentication)
            .Produces<Result<RefreshTokenResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary(nameof(CreateRefreshToken))
            .WithDescription("Refresh The Access-Token");
    }
}