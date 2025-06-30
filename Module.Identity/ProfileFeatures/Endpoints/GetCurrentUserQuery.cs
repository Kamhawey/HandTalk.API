using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Extensions;
using Module.Identity.ProfileFeatures.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.ProfileFeatures.Endpoints;


public class GetCurrentUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(IdentityEndpointConstants.Routes.CurrentUser,
                async (ISender sender) => await sender.Send(new GetCurrentUserQuery()))
            .WithName(nameof(GetCurrentUser))
            .WithTags(IdentityEndpointConstants.Tags.Profile)
            .Produces<Result<UserProfileResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Current User Profile")
            .WithDescription("Get the current authenticated user's profile information")
            .RequireAuthorization();
    }
}
