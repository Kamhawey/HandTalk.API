using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.GetRandomGlosses.Handler;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.GetRandomGlosses.Endpoint;

public class GetRandomGlosses : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(DictionaryEndpointConstants.Routes.GetRandomGlosses,
                async (ISender sender, int count = 6) => await sender.Send(new RandomGlossesQuery(count)))
            .WithName(nameof(GetRandomGlosses))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<List<DictionaryEntryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Random Glosses")
            .WithDescription("Get # Random Glosses ( 6 by Default)");
    }
}