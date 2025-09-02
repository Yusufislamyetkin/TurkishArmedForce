namespace Turna96.SharedKernel.Domain;

public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right)
        => ReferenceEquals(left, right) || left.Equals(right);

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        => !EqualOperator(left, right);

    public abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
        => obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override int GetHashCode()
        => GetEqualityComponents().Aggregate(0, (hash, obj) => HashCode.Combine(hash, obj));
}
