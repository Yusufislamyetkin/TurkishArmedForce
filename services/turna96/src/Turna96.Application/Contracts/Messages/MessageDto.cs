namespace Turna96.Application.Contracts.Messages;

using Turna96.Domain.ValueObjects;

public sealed record MessageDto(
    MessageId MessageId,
    RoomId RoomId,
    UserId SenderId,
    string Body,
    DateTime CreatedAt,
    DateTime? DeliveredAt,
    DateTime? ReadAt,
    DateTime? DeletedAt);
