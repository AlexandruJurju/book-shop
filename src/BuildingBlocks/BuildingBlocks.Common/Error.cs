using Ardalis.Result;

namespace BuildingBlocks.Common;

public sealed record Error(string Code, string Description, ResultStatus Status)
{
    public static Error NotFound(string code, string description)
    {
        return new Error(code, description, ResultStatus.NotFound);
    }

    public static Error Conflict(string code, string description)
    {
        return new Error(code, description, ResultStatus.Conflict);
    }

    public static Error Unauthorized(string code, string description)
    {
        return new Error(code, description, ResultStatus.Unauthorized);
    }

    public static Error Unavailable(string code, string description)
    {
        return new Error(code, description, ResultStatus.Unavailable);
    }

    public static Error CriticalError(string code, string description)
    {
        return new Error(code, description, ResultStatus.CriticalError);
    }

    public Result<T> ToResult<T>()
    {
        return Status switch
        {
            ResultStatus.NotFound => Result<T>.NotFound(Description),
            ResultStatus.Conflict => Result<T>.Conflict(Description),
            ResultStatus.Unauthorized => Result<T>.Unauthorized(Description),
            ResultStatus.Unavailable => Result<T>.Unavailable(Description),
            ResultStatus.CriticalError => Result<T>.CriticalError(Description),
            _ => Result<T>.Error(Description)
        };
    }
}
