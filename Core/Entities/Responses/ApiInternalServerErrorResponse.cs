using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Responses;

public sealed class ApiInternalServerErrorResponse : ApiBaseResponse
{
    public ApiInternalServerErrorResponse(string errorMessage) : base(false,
        StatusCodes.Status500InternalServerError)
    {
        Message = "Internal server error";
        ErrorMessage = errorMessage;
    }

    public string Message { get; set; }
    public string ErrorMessage { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}