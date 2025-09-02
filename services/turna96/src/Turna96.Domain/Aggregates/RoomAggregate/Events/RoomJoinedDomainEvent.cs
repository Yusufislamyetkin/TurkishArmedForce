namespace Turna96.Domain.Aggregates.RoomAggregate.Events;

using Turna96.Domain.ValueObjects;
using Turna96.SharedKernel.Domain;

public record RoomJoinedDomainEvent(RoomId RoomId, UserId UserId, DateTime OccurredOn) : DomainEvent(OccurredOn);
