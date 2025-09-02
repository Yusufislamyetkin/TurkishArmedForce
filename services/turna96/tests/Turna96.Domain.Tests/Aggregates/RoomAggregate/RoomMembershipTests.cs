namespace Turna96.Domain.Tests.Aggregates.RoomAggregate;

using FluentAssertions;
using Turna96.Domain.Aggregates.RoomAggregate;
using Turna96.Domain.Aggregates.RoomAggregate.Events;
using Turna96.Domain.Enums;
using Turna96.Domain.Tests.Fakes;
using Xunit;

public class RoomMembershipTests
{
    [Fact]
    public void Join_ShouldAddMember_AndRaiseEvent()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var room = new Room(TestData.RoomId(), RoomType.Group);

        var result = room.Join(TestData.UserId(), clock);

        result.Succeeded.Should().BeTrue();
        room.Members.Should().Contain(m => m.UserId == TestData.UserId());
        room.DomainEvents.OfType<RoomMemberJoinedDomainEvent>().Should().HaveCount(1);
    }

    [Fact]
    public void Join_ShouldBeIdempotent()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var room = new Room(TestData.RoomId(), RoomType.Group);
        room.Join(TestData.UserId(), clock);

        var eventsBefore = room.DomainEvents.Count;
        room.Join(TestData.UserId(), clock).Succeeded.Should().BeTrue();
        room.DomainEvents.Count.Should().Be(eventsBefore);
    }

    [Fact]
    public void Leave_ShouldRemoveMember_AndRaiseEvent()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var room = new Room(TestData.RoomId(), RoomType.Group);
        room.Join(TestData.UserId(), clock);

        var result = room.Leave(TestData.UserId(), clock);

        result.Succeeded.Should().BeTrue();
        room.Members.Should().BeEmpty();
        room.DomainEvents.OfType<RoomMemberLeftDomainEvent>().Should().HaveCount(1);
    }

    [Fact]
    public void Leave_ShouldBeIdempotent()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var room = new Room(TestData.RoomId(), RoomType.Group);

        var result = room.Leave(TestData.UserId(), clock);

        result.Succeeded.Should().BeTrue();
        room.DomainEvents.Should().BeEmpty();
    }
}
