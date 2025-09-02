namespace Turna96.Domain.Aggregates.MessageAggregate.Events;

using Turna96.Domain.ValueObjects;

public sealed record MessageReadDomainEvent(
    MessageId MessageId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);
