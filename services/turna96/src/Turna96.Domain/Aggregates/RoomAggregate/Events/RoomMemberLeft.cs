namespace Turna96.Domain.Aggregates.RoomAggregate.Events;

public sealed record RoomMemberLeft(RoomId RoomId, UserId UserId, DateTime LeftAt) : IDomainEvent;
