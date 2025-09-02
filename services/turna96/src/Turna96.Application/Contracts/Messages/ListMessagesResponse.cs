namespace Turna96.Application.Contracts.Messages;

public sealed record ListMessagesResponse(IReadOnlyList<MessageDto> Messages, int TotalCount);
