using Microsoft.AspNetCore.Http;

namespace Core.Entities.Responses;

public sealed class ApiNotFoundResponse : ApiBaseResponse
{
    public ApiNotFoundResponse(string message) : base(false, StatusCodes.Status404NotFound)
    {
        Message = message;
    }

    public string Message { get; set; }
}