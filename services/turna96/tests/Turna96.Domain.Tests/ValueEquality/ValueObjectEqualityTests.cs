namespace Turna96.Domain.Tests.ValueEquality;

using FluentAssertions;
using Turna96.Domain.ValueObjects;
using Xunit;

public class ValueObjectEqualityTests
{
    [Fact]
    public void UserId_Equality_By_Value()
    {
        var id = Guid.NewGuid();
        var first = new UserId(id);
        var second = new UserId(id);
        first.Should().Be(second);
    }

    [Fact]
    public void RoomId_Inequality_By_Value()
    {
        var first = new RoomId(Guid.NewGuid());
        var second = new RoomId(Guid.NewGuid());
        first.Should().NotBe(second);
    }
}
