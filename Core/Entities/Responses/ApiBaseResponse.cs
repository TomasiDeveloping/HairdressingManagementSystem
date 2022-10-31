namespace Core.Entities.Responses;

public abstract class ApiBaseResponse
{
    protected ApiBaseResponse(bool success, int statusCode)
    {
        Success = success;
        StatusCode = statusCode;
    }

    public bool Success { get; set; }
    public int StatusCode { get; set; }
}