using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Identity.AuthFeatures.LoginFeature.Handler;
using Module.Identity.Extensions;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Identity;

namespace Module.Identity.AuthFeatures.LoginFeature.Endpoint;

public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(IdentityEndpointConstants.Routes.Login, 
                async (LoginCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(Login))
            .WithTags(IdentityEndpointConstants.Tags.Authentication)
            .Produces<Result<LoginResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary(nameof(Login))
            .WithDescription("sign in");
    }
}