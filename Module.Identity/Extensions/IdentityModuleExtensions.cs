using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Module.Identity.AuthFeatures.ExternalLoginFeature.Config;
using Module.Identity.Domain.Models;
using Module.Identity.Extensions;
using Module.Identity.Infrastructure;
using Module.Identity.Infrastructure.JWT;
using Module.Identity.Infrastructure.Services.JWT;
using Module.Identity.Infrastructure.Services.OTP;
using Module.Identity.Persistence;
using Module.Identity.Persistence.Email;
using Module.Identity.Persistence.JWT;
using Shared.Infrastructure.Email;

namespace Module.Identity.Extensions;

public static class IdentityModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<IdentityModuleDbContext>
                (opt => opt.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddTransient<ITokensService, TokensService>();
        services.AddScoped<IEmailService, EmailService>();
        services
            .AddIdentityServices()
            .AddJwtConfiguration(configuration)
            .AddEmailConfiguration(configuration)
             .AddExternalLoginConfiguration(configuration).AddExternalLoginServices(configuration)
            .AddMemoryCache();
        return services;
    }
    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = nameof(PasswordResetTokenProvider);
                options.Tokens.EmailConfirmationTokenProvider = nameof(EmailOtpTokenProvider);
                options.User.AllowedUserNameCharacters = string.Empty;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
            })
                .AddTokenProvider<EmailOtpTokenProvider>(nameof(EmailOtpTokenProvider))
                .AddTokenProvider<PasswordResetTokenProvider>(nameof(PasswordResetTokenProvider))
                .AddEntityFrameworkStores<IdentityModuleDbContext>().AddDefaultTokenProviders();
            return services;
        } 
    private static void AddAuthenticationServices(this IServiceCollection services,IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            if (tokenValidationParameters is null)
                throw new ArgumentNullException(nameof(tokenValidationParameters), "TokenValidationParameters cannot be null.");

            services.AddDataProtection()
                .SetApplicationName("IdentityModule");

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".AspNetCore.Identity";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Lax; // Critical for OAuth flows
                    options.Cookie.SecurePolicy =
                        CookieSecurePolicy.None; // During dev testing, set to Always in production
                    options.Cookie.Path = "/";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs")))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization();
        }
    private static IServiceCollection AddExternalLoginServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Facebook", c =>
        {
            c.BaseAddress = new Uri(configuration.GetValue<string>("Authentication:Facebook:BaseUrl") ?? string.Empty);
        });
        return services;
    }
    private static IServiceCollection AddExternalLoginConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleAuthConfig>(configuration.GetSection(GoogleAuthConfig.SectionName));
        services.Configure<FacebookAuthConfig>(configuration.GetSection(FacebookAuthConfig.SectionName));
            

        return services;
    }

    private static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSettings jwtSettings = new();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            TokenValidationParameters tokenValidationParameters = TokensService.GetTokenValidationParameters(jwtSettings, true);

            services.AddSingleton(tokenValidationParameters);
            services.AddAuthenticationServices(configuration,tokenValidationParameters);

            return services;
        }
   private static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
         return   services
                .Configure<MailSettings>(configuration.GetSection(MailSettings.SectionName));
        }
}