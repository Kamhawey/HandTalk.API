using Microsoft.Extensions.DependencyInjection;
using Shared.Features.FileUploads;
using Shared.Infrastructure.Services;

namespace Shared.Extensions;

public static class SharedModuleExtensions
{
    public static IServiceCollection AddSharedModuleServices(this IServiceCollection services)
    
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileUploadService,FileUploadService>();
        return services;
    }
}