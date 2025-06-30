using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.Update.Handler;
using Shared.DTOs.Common;
using Shared.DTOs.Dictionary;

namespace Module.Dictionary.Features.Update.Endpoint;

public class Update : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(DictionaryEndpointConstants.Routes.Update, 
                async (UpdateDictionaryEntryCommand command, ISender sender) => await sender.Send(command))
            .WithName(nameof(Update))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<Result<DictionaryEntryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Dictionary Entry")
            .WithDescription("Update an existing dictionary entry");
    }
}