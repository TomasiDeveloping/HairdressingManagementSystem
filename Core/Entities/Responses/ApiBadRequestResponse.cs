namespace Core.Entities.Responses;

public sealed class ApiBadRequestResponse : ApiBaseResponse
{
    public ApiBadRequestResponse(string message, string? error = null) : base(false, 400)
    {
        Message = message;
        Error = error;
    }

    public string Message { get; set; }
    public string? Error { get; set; }
}