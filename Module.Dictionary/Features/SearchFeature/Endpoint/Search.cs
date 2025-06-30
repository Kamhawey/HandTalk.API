using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.SearchFeature.Handler;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Dictionary;
using Shared.Infrastructure.Services;

namespace Module.Dictionary.Features.SearchFeature.Endpoint;

public class Search : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(DictionaryEndpointConstants.Routes.Search, 
                async (SearchDictionaryDto searchDict  ,ICurrentUserService currentUser, ISender sender) => await sender.Send(new SearchDictionaryQuery{SearchDictionary = searchDict,UserId = currentUser.Id}))
            .WithName(nameof(Search))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<PagedResult<DictionaryEntryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Search Dictionary Entries")
            .WithDescription("Search for dictionary entries using various matching criteria");
    }
}