namespace Turna96.Domain.Tests.Aggregates.MessageAggregate;

using FluentAssertions;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Errors;
using Turna96.Domain.Tests.Fakes;
using Turna96.Domain.ValueObjects;
using Xunit;

public class MessageSequenceTests
{
    [Fact]
    public void AssignSequence_ShouldSet_WhenUnset()
    {
        var message = CreateMessage();
        message.AssignSequence(1).Succeeded.Should().BeTrue();
        message.Sequence.Should().Be(1);
    }

    [Fact]
    public void AssignSequence_ShouldBeIdempotent_ForSameValue()
    {
        var message = CreateMessage();
        message.AssignSequence(1);
        message.AssignSequence(1).Succeeded.Should().BeTrue();
        message.Sequence.Should().Be(1);
    }

    [Fact]
    public void AssignSequence_ShouldFail_ForDifferentValue()
    {
        var message = CreateMessage();
        message.AssignSequence(1);
        var result = message.AssignSequence(2);
        result.Succeeded.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Message.SequenceMismatch(1, 2));
    }

    private static Message CreateMessage()
    {
        var clock = new FakeClock(DateTime.UtcNow);
        var body = MessageBody.Create("hi").Value!;
        return Message.Create(TestData.MessageId(), TestData.UserId(), TestData.RoomId(), body, clock).Value!;
    }
}
