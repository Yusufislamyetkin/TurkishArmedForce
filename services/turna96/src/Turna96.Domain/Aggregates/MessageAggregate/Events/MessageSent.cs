namespace Turna96.Domain.Aggregates.MessageAggregate.Events;

public sealed record MessageSent(MessageId MessageId, RoomId RoomId, UserId SenderId, long Sequence, DateTime SentAt) : IDomainEvent;
