namespace Turna96.Application.Contracts.DTOs;

public sealed record MessageDto(string MessageId, string SenderId, string Body, DateTime SentAt);
