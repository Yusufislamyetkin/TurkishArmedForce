namespace Turna96.Application.Contracts.Messages;

using Turna96.Domain.ValueObjects;

public sealed record SendMessageResponse(MessageId MessageId, long Sequence, DateTime CreatedAt);
