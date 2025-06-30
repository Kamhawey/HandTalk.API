using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Module.Identity.Domain.Models;

namespace Module.Identity.Infrastructure.Services.OTP
{
    public class PasswordResetTokenProvider(IMemoryCache cache) : IUserTwoFactorTokenProvider<ApplicationUser>
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        public Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var otp = Enumerable.Range(1, 6)
                .Select(_ => Random.Shared.Next(0, 9).ToString())
                .Aggregate((acc, num) => acc + num);

            cache.Set($"PasswordReset-{user.Id}", otp, TimeSpan.FromMinutes(15));

            return Task.FromResult(otp);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var otp = cache.Get<string>($"PasswordReset-{user.Id}");

            if (otp == null)
                return Task.FromResult(false);

            return Task.FromResult(otp == token);
        }
    }
}
