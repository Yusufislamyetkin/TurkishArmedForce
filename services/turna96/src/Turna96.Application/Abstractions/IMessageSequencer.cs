namespace Turna96.Application.Abstractions;

using Turna96.Domain.ValueObjects;

public interface IMessageSequencer
{
    Task<long> NextAsync(RoomId roomId, CancellationToken ct = default);
}
