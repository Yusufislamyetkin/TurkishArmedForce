namespace Turna96.Domain.Tests.Aggregates.MessageAggregate;

using FluentAssertions;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Aggregates.MessageAggregate.Events;
using Turna96.Domain.Services;
using Turna96.Domain.Tests.Fakes;
using Turna96.Domain.ValueObjects;
using Xunit;

public class MessageDeliveryReadTests
{
    [Fact]
    public void MarkDelivered_ShouldRaiseEvent_WhenFirstTime()
    {
        var clock = new FakeClock(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        var message = CreateMessage(clock);
        var result = message.MarkDelivered(clock);

        result.Succeeded.Should().BeTrue();
        message.DeliveredAt.Should().Be(clock.UtcNow);
        message.DomainEvents.OfType<MessageDeliveredDomainEvent>().Should().HaveCount(1);
    }

    [Fact]
    public void MarkDelivered_ShouldBeIdempotent()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var message = CreateMessage(clock);
        message.MarkDelivered(clock);
        var eventsBefore = message.DomainEvents.Count;
        message.MarkDelivered(clock).Succeeded.Should().BeTrue();
        message.DomainEvents.Count.Should().Be(eventsBefore);
    }

    [Fact]
    public void MarkRead_ShouldRaiseEvent_WhenFirstTime()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var message = CreateMessage(clock);
        var result = message.MarkRead(clock);

        result.Succeeded.Should().BeTrue();
        message.ReadAt.Should().Be(clock.UtcNow);
        message.DomainEvents.OfType<MessageReadDomainEvent>().Should().HaveCount(1);
    }

    [Fact]
    public void MarkRead_ShouldBeIdempotent()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var message = CreateMessage(clock);
        message.MarkRead(clock);
        var eventsBefore = message.DomainEvents.Count;
        message.MarkRead(clock).Succeeded.Should().BeTrue();
        message.DomainEvents.Count.Should().Be(eventsBefore);
    }

    [Fact]
    public void DeletedMessage_ShouldIgnore_Delivery_And_Read()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var message = CreateMessage(clock);
        message.Delete(clock);
        var eventsBefore = message.DomainEvents.Count;
        message.MarkDelivered(clock).Succeeded.Should().BeTrue();
        message.MarkRead(clock).Succeeded.Should().BeTrue();
        message.DomainEvents.Count.Should().Be(eventsBefore);
    }

    private static Message CreateMessage(IClock clock)
    {
        var body = MessageBody.Create("hi").Value!;
        return Message.Create(TestData.MessageId(), TestData.UserId(), TestData.RoomId(), body, clock).Value!;
    }
}
