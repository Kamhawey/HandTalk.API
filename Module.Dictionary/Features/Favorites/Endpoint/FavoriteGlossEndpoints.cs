using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.JsonWebTokens;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.Favorites.Handlers;
using Shared.DTOs.Common;
using Shared.DTOs.Dictionary.Favorites;
using Shared.Infrastructure.Services;

namespace Module.Dictionary.Features.Favorites.Endpoint;

  public class FavoriteGlossEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(DictionaryEndpointConstants.Routes.GetUserFavorites, 
                    async (ISender sender, ICurrentUserService currentUserService) => {
                        var currentUserId = currentUserService.Id;
                        return currentUserId is null ? Results.Unauthorized() : Results.Ok(await sender.Send(new GetUserFavoritesQuery(currentUserId.Value)));
                    })
                .WithName("GetUserFavorites")
                .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
                .RequireAuthorization()
                .Produces<List<UserFavoriteGlossDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithSummary("Get User's Favorite Glosses")
                .WithDescription("Retrieves all favorite glosses for the authenticated user")
                .RequireAuthorization();

            
            app.MapPost(DictionaryEndpointConstants.Routes.ToggleFavorite, 
                    async ( long glossId,ISender sender,ICurrentUserService currentUserService) =>
                    {
                        var currentUserId = currentUserService.Id;
                        if (currentUserId is null ) return Results.Unauthorized();
                        
                        var command = new ToggleFavoriteGlossCommand
                        {
                            UserId = currentUserId.Value,
                            DictionaryEntryId = glossId
                        };
                        var result = await sender.Send(command);
                        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
                    })
                .WithName("ToggleFavoriteGloss")
                .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
                .RequireAuthorization()
                .Produces<Result<bool>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .WithSummary("Toggle Favorite Gloss")
                .WithDescription("Add or remove a gloss from the user's favorites")
                .RequireAuthorization();
        }
        private string? GetUserId (HttpContext httpContext) => 
            httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    }