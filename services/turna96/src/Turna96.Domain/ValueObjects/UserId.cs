namespace Turna96.Domain.ValueObjects;

public sealed class UserId : ValueObject
{
    public UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static UserId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
