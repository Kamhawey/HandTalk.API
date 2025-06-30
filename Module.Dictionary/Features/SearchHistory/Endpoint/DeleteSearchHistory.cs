using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.SearchHistory.Handler;
using Shared.Infrastructure.Services;

namespace Module.Dictionary.Features.SearchHistory.Endpoint;

public class DeleteSearchHistory : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(DictionaryEndpointConstants.Routes.SearchHistory + "/{id:long}", 
                async (long id, ICurrentUserService currentUser, ISender sender) => await sender.Send(new DeleteSearchHistoryCommand
                {
                    SearchHistoryId = id,
                    UserId = currentUser.Id
                }))
            .WithName("DeleteSingleSearchHistory")
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Single Search History Entry")
            .WithDescription("Deletes a specific search history entry for the current user")
            .RequireAuthorization();


        app.MapDelete(DictionaryEndpointConstants.Routes.SearchHistory + "/all", 
                async (ICurrentUserService currentUser, ISender sender) => await sender.Send(new DeleteAllSearchHistoryCommand
                {
                    UserId = currentUser.Id
                }))
            .WithName("DeleteAllSearchHistory")
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete All Search History")
            .WithDescription("Deletes all search history entries for the current user")
            .RequireAuthorization();
        
    }
}
