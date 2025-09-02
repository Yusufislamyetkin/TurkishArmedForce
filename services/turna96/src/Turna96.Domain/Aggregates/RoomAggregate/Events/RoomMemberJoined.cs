namespace Turna96.Domain.Aggregates.RoomAggregate.Events;

public sealed record RoomMemberJoined(RoomId RoomId, UserId UserId, DateTime JoinedAt) : IDomainEvent;
