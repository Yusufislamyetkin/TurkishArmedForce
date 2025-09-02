namespace Turna96.Domain.Aggregates.MessageAggregate.Events;

using Turna96.Domain.ValueObjects;

public sealed record MessageSentDomainEvent(
    MessageId MessageId,
    RoomId RoomId,
    UserId SenderId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);
