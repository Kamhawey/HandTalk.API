using Microsoft.OpenApi.Models;

namespace API.Extensions;

    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.OrderActionsBy((apiDesc) =>
                {
                    var methodOrder = new Dictionary<string, int>
                    {
                        { "GET", 0 },
                        { "POST", 1 },
                        { "PUT", 2 },
                        { "DELETE", 3 }
                    };

                    var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
                    var method = apiDesc.HttpMethod.ToUpper();

                    int methodPriority = methodOrder.ContainsKey(method)
                        ? methodOrder[method]
                        : int.MaxValue;

                    return $"{controller}_{methodPriority}_{apiDesc.RelativePath}";
                });

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "HandTalk API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",

                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        []
                    }
                });
            });

            return services;
        }
    }