using System.Linq;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Aggregates.MessageAggregate.Events;
using Turna96.Domain.Tests.Fakes;
using Xunit;

namespace Turna96.Domain.Tests.Aggregates.MessageAggregate;

public class MessageDeliveryReadTests
{
    [Fact]
    public void MarkDelivered_Should_Be_Idempotent()
    {
        var clock = new FakeClock();
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);
        message.SetSequence(1);

        message.MarkDelivered(clock);
        message.MarkDelivered(clock);

        Assert.Equal(1, message.DomainEvents.OfType<MessageDelivered>().Count());
    }

    [Fact]
    public void MarkRead_Should_Be_Idempotent()
    {
        var clock = new FakeClock();
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);
        message.SetSequence(1);

        message.MarkRead(clock);
        message.MarkRead(clock);

        Assert.Equal(1, message.DomainEvents.OfType<MessageRead>().Count());
    }

    [Fact]
    public void Deleted_Message_Should_Not_Raise_Delivery_Or_Read_Events()
    {
        var clock = new FakeClock();
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);
        message.SetSequence(1);
        message.Delete(clock);

        message.MarkDelivered(clock);
        message.MarkRead(clock);

        Assert.Empty(message.DomainEvents.OfType<MessageDelivered>());
        Assert.Empty(message.DomainEvents.OfType<MessageRead>());
    }
}
