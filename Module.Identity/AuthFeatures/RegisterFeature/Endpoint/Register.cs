using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.Extensions;
using Module.Identity.Features.RegisterFeature.Handler;
using Shared.DTOs.Common;

namespace Module.Identity.Features.RegisterFeature.Endpoint;

public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.Register,
                async (RegisterCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(Register))
            .WithTags(IdentityEndpointConstants.Tags.Authentication)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary(nameof(Register))
            .WithDescription("sign up");
    }
}