namespace Turna96.Domain.Services;

using Turna96.Domain.ValueObjects;

public interface IMessageSequencer
{
    Task<long> NextAsync(RoomId roomId, CancellationToken cancellationToken);
}
