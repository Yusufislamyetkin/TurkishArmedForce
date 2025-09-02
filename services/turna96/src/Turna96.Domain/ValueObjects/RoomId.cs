namespace Turna96.Domain.ValueObjects;

public sealed record RoomId(Guid Value) : ValueObject
{
    public static RoomId New() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
