namespace Turna96.Domain.Common;

public abstract record ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public virtual bool Equals(ValueObject? other)
    {
        if (other is null || other.GetType() != GetType()) return false;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(0, (current, obj) => HashCode.Combine(current, obj));
    }
}
