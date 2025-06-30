using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.DTOs.Common;
using Shared.DTOs.Files;
using Shared.Extensions;
using Shared.Features.FileUploads.Handler;

namespace Shared.Features.FileUploads.Endpoint;

public class UploadFile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(EdnpointsConstants.Routes.UploadFile,
                async (IFormFile file, string? folder, ISender sender) => 
                    await sender.Send(new UploadFileCommand(file, folder)))
            .WithName(nameof(UploadFile))
            .WithTags(EdnpointsConstants.Tags.Files)
            .Produces<Result<FileUploadResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status413PayloadTooLarge)
            .WithSummary("Upload File")
            .WithDescription("Upload a file to the server")
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}