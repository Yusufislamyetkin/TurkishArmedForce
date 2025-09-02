namespace Turna96.Domain.ValueObjects;

public sealed record MessageId(Guid Value) : ValueObject
{
    public static MessageId New() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
