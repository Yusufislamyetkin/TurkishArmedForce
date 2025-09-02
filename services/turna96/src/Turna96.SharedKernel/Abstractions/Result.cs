namespace Turna96.SharedKernel.Abstractions;

public class Result
{
    private Result(bool succeeded, Error? error)
    {
        Succeeded = succeeded;
        Error = error;
    }

    public bool Succeeded { get; }

    public Error? Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(Error error) => new(false, error);
}
