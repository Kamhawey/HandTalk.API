namespace Shared.DTOs.Common;

public static class RegexPatterns
{
    public const string EmailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
    public const string UsernamePattern = @"^[a-zA-Z0-9_]*$";
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";
}