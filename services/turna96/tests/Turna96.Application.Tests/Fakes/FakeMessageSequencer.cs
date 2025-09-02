namespace Turna96.Application.Tests.Fakes;

using Turna96.Application.Abstractions;
using Turna96.Domain.ValueObjects;

public sealed class FakeMessageSequencer : IMessageSequencer
{
    private long _current;

    public Task<long> NextAsync(RoomId roomId, CancellationToken ct = default)
    {
        _current++;
        return Task.FromResult(_current);
    }
}
