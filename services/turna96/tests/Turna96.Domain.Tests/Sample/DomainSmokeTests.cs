using FluentAssertions;
using Xunit;

namespace Turna96.Domain.Tests.Sample;

public class DomainSmokeTests
{
    [Fact]
    public void True_ShouldBeTrue()
    {
        true.Should().BeTrue();
    }
}
