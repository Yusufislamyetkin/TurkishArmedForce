namespace Turna96.Domain.Aggregates.MessageAggregate.Events;

public sealed record MessageDelivered(MessageId MessageId, RoomId RoomId, long Sequence, DateTime DeliveredAt) : IDomainEvent;
