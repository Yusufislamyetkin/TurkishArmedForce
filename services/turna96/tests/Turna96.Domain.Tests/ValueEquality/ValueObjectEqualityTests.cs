using Turna96.Domain.ValueObjects;
using Xunit;

namespace Turna96.Domain.Tests.ValueEquality;

public class ValueObjectEqualityTests
{
    [Fact]
    public void UserId_WithSameGuid_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var first = new UserId(id);
        var second = new UserId(id);

        Assert.Equal(first, second);
    }
}
