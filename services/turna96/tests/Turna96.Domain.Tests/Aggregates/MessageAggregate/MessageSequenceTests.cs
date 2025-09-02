using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Errors;
using Turna96.Domain.Tests.Fakes;
using Xunit;

namespace Turna96.Domain.Tests.Aggregates.MessageAggregate;

public class MessageSequenceTests
{
    [Fact]
    public void SetSequence_FirstTime_Should_RaiseEvent()
    {
        var clock = new FakeClock();
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);

        var result = message.SetSequence(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, message.Sequence);
        Assert.Single(message.DomainEvents);
    }

    [Fact]
    public void SetSequence_SameValue_Should_Be_Idempotent()
    {
        var clock = new FakeClock();
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);
        message.SetSequence(1);

        var result = message.SetSequence(1);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, message.Sequence);
        Assert.Single(message.DomainEvents);
    }

    [Fact]
    public void SetSequence_DifferentValue_Should_Fail()
    {
        var clock = new FakeClock();
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);
        message.SetSequence(1);

        var result = message.SetSequence(2);

        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Message.SequenceMismatch, result.Error);
    }
}
