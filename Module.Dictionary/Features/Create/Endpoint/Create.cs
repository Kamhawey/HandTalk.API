using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.Create.Handler;
using Shared.DTOs.Common;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.Create.Endpoint;

public class Create : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(DictionaryEndpointConstants.Routes.Create, 
                async (CreateDictionaryEntryCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(Create))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<Result<DictionaryEntryDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Dictionary Entry")
            .WithDescription("Create a new dictionary entry with gloss and video URL");
    }
}