namespace Turna96.Domain.Aggregates.MessageAggregate.Events;

using Turna96.Domain.ValueObjects;
using Turna96.SharedKernel.Domain;

public record MessageSentDomainEvent(MessageId MessageId, DateTime OccurredOn) : DomainEvent(OccurredOn);
