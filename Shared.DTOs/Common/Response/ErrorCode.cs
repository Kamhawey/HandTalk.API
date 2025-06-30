namespace Shared.DTOs.Common.Response;

public enum ErrorCode
{
    None = 0,
    ValidationError = 1,
    VerificationCodeNotValid = 2,
    NotFound = 3,
    FileRequired = 4,
    FileSizeExceedsLimit =5,
    FileUploadFailed,
    InvalidImageFile,
    FileDeleteFailed,
    // Add other existing error codes

    // Identity error codes
    InvalidCredentials = 1000,
    EmailNotConfirmed = 1001,
    UserNotFound = 1002,
    EmailAlreadyExists = 1003,
    UsernameAlreadyExists = 1004,
    RegistrationFailed = 1005,
    InvalidToken = 1006,
    TokenCreationFailed = 1007,
    UnregisteredEmail = 1008,
    ExternalLoginFailed ,
    ExternalLoginEmailRequired ,
    UnsupportedExternalProvider ,
    FailedToUpdateData ,

    
    HandTalkServiceUnavailable = 2000,
    HandTalkGlossNotFound = 2001,
    HandTalkProcessingError = 2002,
    HandTalkInvalidInput = 2003,
    HandTalkRateLimitExceeded = 2004,
    HandTalkEmptyResponse = 2005,
    
    DictionaryEntryAlreadyExists = 3000,
    DictionaryEntryNotExists = 3001,
    MaximumFavoritesReached = 3002,
}