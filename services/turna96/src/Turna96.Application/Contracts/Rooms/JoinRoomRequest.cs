namespace Turna96.Application.Contracts.Rooms;

using Turna96.Domain.ValueObjects;

public sealed record JoinRoomRequest(RoomId RoomId, UserId UserId);
