using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Dictionary.Extensions;
using Module.Dictionary.Features.Delete.Handler;
using Shared.DTOs.Common;

namespace Module.Dictionary.Features.Delete.Endpoint;

public class Delete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(DictionaryEndpointConstants.Routes.Delete, 
                async (long id, ISender sender) => await sender.Send(new DeleteDictionaryEntryCommand(id)))
            .WithName(nameof(Delete))
            .WithTags(DictionaryEndpointConstants.Tags.Dictionary)
            .Produces<Result>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Dictionary Entry")
            .WithDescription("Delete an existing dictionary entry");
    }
}