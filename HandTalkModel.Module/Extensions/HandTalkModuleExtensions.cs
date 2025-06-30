using HandTalkModel.Module.Infrastructure.Services;
using HandTalkModel.Module.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HandTalkModel.Module.Extensions;

public static class HandTalkModuleExtensions
{
    public static IServiceCollection AddHandTalkModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HandTalkOptions>(configuration.GetSection(HandTalkOptions.SectionName));
        services.AddGrpc();
        services.AddSingleton<IHandTalkService, HandTalkService>();
        
        return services;
    }
}