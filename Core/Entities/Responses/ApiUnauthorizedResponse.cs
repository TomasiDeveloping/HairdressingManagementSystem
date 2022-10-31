using Microsoft.AspNetCore.Http;

namespace Core.Entities.Responses;

public class ApiUnauthorizedResponse : ApiBaseResponse
{
    public ApiUnauthorizedResponse(string? errorMessage) : base(false, StatusCodes.Status401Unauthorized)
    {
        ErrorMessage = errorMessage;
    }

    public string? ErrorMessage { get; set; }
}