using System.Reflection;
using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Extensions;

public static class CarterExtensions
{
    public static IServiceCollection AddCarterModules(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddCarter(configurator: cfg =>
        {
            foreach (Assembly assembly in assemblies)
            {
                var module = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(ICarterModule)))
                    .ToArray();
            cfg.WithModules(module);
            }
        }); 
        return services;
    }
}