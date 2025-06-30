using Shared.DTOs.Common.Response;

namespace Shared.DTOs.Common;

public class Error
{
    public Error(ErrorCode code)
    {
        Code = code;
        Message = code.ToString();
    }
    
    public Error(ErrorCode code , string message)
    {
        Code = code;
        Message = message;
    }

    public ErrorCode Code { get; }
    public string Message { get; }

    public static Error None => new(ErrorCode.None);

    public static implicit operator Error(ErrorCode code) => new(code);
}
