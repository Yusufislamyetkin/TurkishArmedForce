using FluentAssertions;
using Xunit;

namespace Turna96.Application.Tests.Sample;

public class ApplicationSmokeTests
{
    [Fact]
    public void True_ShouldBeTrue()
    {
        true.Should().BeTrue();
    }
}
