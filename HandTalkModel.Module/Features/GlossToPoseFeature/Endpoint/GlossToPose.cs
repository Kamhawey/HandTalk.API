using Carter;
using HandTalkModel.Module.Extensions;
using HandTalkModel.Module.Features.GlossToPoseFeature.Handler;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.DTOs.HandTalk;

namespace HandTalkModel.Module.Features.GlossToPoseFeature.Endpoint;

public class GlossToPose : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(HandTalkEndpointConstants.Routes.GlossToPose,
                async (GlossToPoseQuery query, ISender sender) => await sender.Send(query))
            .WithName(nameof(GlossToPose))
            .WithTags(HandTalkEndpointConstants.Tags.HandTalk)
            .Produces<GlossToPoseResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Convert text to sign language pose")
            .WithDescription("Converts a text gloss to a sign language pose visualization");

        app.MapGet(HandTalkEndpointConstants.Routes.Health,
                async (ISender sender) => await sender.Send(new HandTalkHealthQuery()))
            .WithName("HandTalkHealth")
            .WithTags(HandTalkEndpointConstants.Tags.Health)
            .Produces<bool>(StatusCodes.Status200OK)
            .WithSummary("Check HandTalk service health")
            .WithDescription("Checks if the HandTalk service is healthy and responding to requests");

        app.MapGet(HandTalkEndpointConstants.Routes.Version,
                async (ISender sender) => await sender.Send(new HandTalkVersionQuery()))
            .WithName("HandTalkVersion")
            .WithTags(HandTalkEndpointConstants.Tags.HandTalk)
            .Produces<string>(StatusCodes.Status200OK)
            .WithSummary("Get HandTalk service version")
            .WithDescription("Returns the current version of the HandTalk service");
    }
}