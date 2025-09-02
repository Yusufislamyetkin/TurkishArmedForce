namespace Turna96.Domain.ValueObjects;

using Turna96.SharedKernel.Domain;

public sealed class RoomId : ValueObject
{
    public RoomId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static RoomId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
