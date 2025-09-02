namespace Turna96.SharedKernel.Abstractions;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

public static class Errors
{
    public static Error Validation(string description) => new("validation", description);
}
