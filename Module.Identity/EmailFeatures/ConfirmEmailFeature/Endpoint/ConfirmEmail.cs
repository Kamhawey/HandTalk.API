using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.EmailFeatures.ConfirmEmailFeature.Handler;
using Module.Identity.Extensions;
using Shared.DTOs.Common;

namespace Module.Identity.EmailFeatures.ConfirmEmailFeature.Endpoint;

public class ConfirmEmail : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(IdentityEndpointConstants.Routes.ConfirmEmail,
                async (ConfirmEmailCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(ConfirmEmail))
            .WithTags(IdentityEndpointConstants.Tags.Email)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary(nameof(ConfirmEmail))
            .WithDescription("Confirm user email with verification code");
    }
}