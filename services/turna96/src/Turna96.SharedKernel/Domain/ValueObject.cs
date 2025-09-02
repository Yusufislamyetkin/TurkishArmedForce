namespace Turna96.SharedKernel.Domain;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj) =>
        obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(0, (hash, obj) => HashCode.Combine(hash, obj));

    public bool Equals(ValueObject? other) => Equals(other as object);
}
