namespace Turna96.Domain.Aggregates.MessageAggregate.Events;

public sealed record MessageRead(MessageId MessageId, RoomId RoomId, long Sequence, DateTime ReadAt) : IDomainEvent;
