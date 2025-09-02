namespace Turna96.Domain.ValueObjects;

public sealed record UserId(Guid Value) : ValueObject
{
    public static UserId New() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
