using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Tests.Fakes;
using Xunit;

namespace Turna96.Domain.Tests.Aggregates.MessageAggregate;

public class MessageCreationTests
{
    [Fact]
    public void Create_Should_Set_CreatedAt_And_NoSequence()
    {
        var fixedTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(fixedTime);
        var message = Message.Create(TestData.MessageId, TestData.RoomId, TestData.UserA, TestData.ValidBody, null, clock);

        Assert.Equal(fixedTime, message.CreatedAt);
        Assert.Null(message.Sequence);
        Assert.Empty(message.DomainEvents);
    }
}
