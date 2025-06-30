using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Extensions;
using Module.Identity.ProfileFeatures.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.ProfileFeatures.Endpoints;

public class EditProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(IdentityEndpointConstants.Routes.EditProfile,
                async (EditProfileCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(EditProfile))
            .WithTags(IdentityEndpointConstants.Tags.Profile)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Edit User Profile")
            .WithDescription("Update the current user's profile information")
            .RequireAuthorization();
    }
}