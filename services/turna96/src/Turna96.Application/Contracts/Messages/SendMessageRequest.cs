namespace Turna96.Application.Contracts.Messages;

using Turna96.Domain.ValueObjects;

public sealed record SendMessageRequest(RoomId RoomId, string Body, string? MetadataJson);
