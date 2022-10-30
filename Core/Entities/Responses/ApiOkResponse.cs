namespace Core.Entities.Responses;

public sealed class ApiOkResponse<TResult> : ApiBaseResponse
{
    public ApiOkResponse(TResult result) : base(true, 200)
    {
        Result = result;
    }

    public TResult Result { get; set; }
}