namespace Turna96.SharedKernel.Domain;

public abstract record DomainEvent(DateTime OccurredOn) : IDomainEvent;
