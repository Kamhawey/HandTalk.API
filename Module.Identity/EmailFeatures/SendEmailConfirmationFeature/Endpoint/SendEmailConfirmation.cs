using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.EmailFeatures.Handler;
using Module.Identity.Extensions;
using Shared.DTOs.Common;

namespace Module.Identity.EmailFeatures.Endpoint;

public class SendEmailConfirmation : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(IdentityEndpointConstants.Routes.SendEmailConfirmation,
                async (SendEmailConfirmationCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(SendEmailConfirmation))
            .WithTags(IdentityEndpointConstants.Tags.Email)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary(nameof(SendEmailConfirmation))
            .WithDescription("Send email confirmation to user");
    }
}