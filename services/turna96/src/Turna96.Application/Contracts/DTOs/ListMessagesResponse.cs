namespace Turna96.Application.Contracts.DTOs;

public sealed record ListMessagesResponse(IReadOnlyList<MessageDto> Messages);
