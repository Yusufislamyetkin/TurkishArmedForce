using System.Linq;
using Turna96.Domain.Aggregates.RoomAggregate;
using Turna96.Domain.Aggregates.RoomAggregate.Events;
using Turna96.Domain.Enums;
using Turna96.Domain.Tests.Fakes;
using Xunit;

namespace Turna96.Domain.Tests.Aggregates.RoomAggregate;

public class RoomMembershipTests
{
    [Fact]
    public void Join_Should_Be_Idempotent()
    {
        var clock = new FakeClock();
        var room = Room.Create(TestData.RoomId, RoomType.Group);

        var first = room.Join(TestData.UserA, clock);
        var second = room.Join(TestData.UserA, clock);

        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        Assert.Single(room.Members);
        Assert.Equal(1, room.DomainEvents.OfType<RoomMemberJoined>().Count());
    }

    [Fact]
    public void Leave_Should_Be_Idempotent()
    {
        var clock = new FakeClock();
        var room = Room.Create(TestData.RoomId, RoomType.Group);

        room.Join(TestData.UserA, clock);
        var first = room.Leave(TestData.UserA, clock);
        var second = room.Leave(TestData.UserA, clock);

        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        Assert.Empty(room.Members);
        Assert.Equal(2, room.DomainEvents.Count);
        Assert.Equal(1, room.DomainEvents.OfType<RoomMemberLeft>().Count());
    }
}
