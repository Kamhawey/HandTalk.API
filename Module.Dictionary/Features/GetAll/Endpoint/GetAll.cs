using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.GetAll.Handler;
using Shared.DTOs.Common.Response;
using Shared.DTOs.Dictionary;
using Shared.Infrastructure.Services;

namespace Module.Dictionary.Features.GetAll.Endpoint;

public class GetAll : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(DictionaryEndpointConstants.Routes.GetAll, 
                async (int pageNumber, int pageSize, ICurrentUserService currentUser, ISender sender) => 
                    await sender.Send(new GetAllDictionaryEntriesQuery
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        UserId = currentUser.Id
                    }))
            .WithName(nameof(GetAll))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<PagedResult<DictionaryEntryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get All Dictionary Entries")
            .WithDescription("Retrieves dictionary entries in alphabetical order with pagination for infinite scrolling");
    }
}