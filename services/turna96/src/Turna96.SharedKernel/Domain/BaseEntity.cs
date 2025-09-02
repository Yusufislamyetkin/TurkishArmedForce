namespace Turna96.SharedKernel.Domain;

public abstract class BaseEntity<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public TId Id { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
