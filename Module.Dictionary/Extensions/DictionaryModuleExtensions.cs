using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Dictionary.Infrastructure;

namespace Module.Dictionary.Extensions;

public static class DictionaryModuleExtensions
{
    public static IServiceCollection AddDictionaryModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DictionaryModuleDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("Database")));
                
        return services;
    }
}