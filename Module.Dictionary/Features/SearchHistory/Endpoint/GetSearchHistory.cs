using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.SearchHistory.Handler;
using Shared.DTOs.Dictionary.SearchHistory;
using Shared.Infrastructure.Services;

namespace Module.Dictionary.Features.SearchHistory.Endpoint;

public class GetSearchHistory : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(DictionaryEndpointConstants.Routes.SearchHistory + "/{count:int?}", 
                async (int? count, ICurrentUserService currentUser, ISender sender) => 
                {
                    return Results.Ok(await sender.Send(new GetUserSearchHistoryQuery
                    {
                        UserId = currentUser.Id,
                        Count = count ?? 10
                    }));
                })
            .WithName(nameof(GetSearchHistory))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<List<UserSearchHistoryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("Get User Search History")
            .WithDescription("Retrieves the user's most recent dictionary searches")
            .RequireAuthorization();
    }
}