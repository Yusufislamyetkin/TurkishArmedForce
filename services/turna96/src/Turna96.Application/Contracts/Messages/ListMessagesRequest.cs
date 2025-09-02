namespace Turna96.Application.Contracts.Messages;

using Turna96.Domain.ValueObjects;

public sealed record ListMessagesRequest(RoomId RoomId, int Page, int PageSize, long? AfterSequence);
