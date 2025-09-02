namespace Turna96.Domain.Tests.Aggregates.MessageAggregate;

using FluentAssertions;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Aggregates.MessageAggregate.Events;
using Turna96.Domain.Tests.Fakes;
using Turna96.Domain.ValueObjects;
using Xunit;

public class MessageCreationTests
{
    [Fact]
    public void Create_ShouldRaiseEvent()
    {
        var clock = new FakeClock(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        var body = MessageBody.Create("hi").Value!;
        var result = Message.Create(TestData.MessageId(), TestData.UserId(), TestData.RoomId(), body, clock);

        result.Succeeded.Should().BeTrue();
        var message = result.Value!;
        message.CreatedAt.Should().Be(clock.UtcNow);
        message.DomainEvents.Should().HaveCount(1);
        message.DomainEvents.Single().Should().BeOfType<MessageSentDomainEvent>();
    }
}
