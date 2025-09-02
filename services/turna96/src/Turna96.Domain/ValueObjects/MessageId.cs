namespace Turna96.Domain.ValueObjects;

public sealed class MessageId : ValueObject
{
    public MessageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static MessageId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
