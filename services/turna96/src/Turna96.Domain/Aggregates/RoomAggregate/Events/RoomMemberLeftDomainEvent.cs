namespace Turna96.Domain.Aggregates.RoomAggregate.Events;

using Turna96.Domain.ValueObjects;

public sealed record RoomMemberLeftDomainEvent(
    RoomId RoomId,
    UserId UserId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);
