using Core.Models;

namespace Core.Helpers.Services;

public class ErrorService
{
    public static ErrorDetail CreateError(string message, int statusCode, string exceptionMessage = "")
    {
        return new ErrorDetail
        {
            StatusCode = statusCode,
            Message = message,
            ExceptionMessage = exceptionMessage
        };
    }
}