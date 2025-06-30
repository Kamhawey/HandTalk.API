using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Module.Identity.Infrastructure.Services.OTP.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var attribute = enumValue.GetType()
                                     .GetMember(enumValue.ToString())
                                     .FirstOrDefault()?
                                     .GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? enumValue.ToString();
        }

    }
}
