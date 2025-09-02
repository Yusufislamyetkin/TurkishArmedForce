namespace Turna96.SharedKernel.Domain;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}
