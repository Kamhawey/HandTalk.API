using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.GetPopularGlosses.Handler;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.GetPopularGlosses.Endpoint;

public class GetPopularGlosses : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(DictionaryEndpointConstants.Routes.GetPopularGlosses, 
                async (int count, ISender sender) => await sender.Send(new RandomGlossesQuery(count)))
            .WithName(nameof(GetPopularGlosses))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<List<DictionaryEntryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Popular Glosses")
            .WithDescription("Get the most searched dictionary entries");
    }
}