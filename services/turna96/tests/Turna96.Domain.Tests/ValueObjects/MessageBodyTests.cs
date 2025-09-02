namespace Turna96.Domain.Tests.ValueObjects;

using FluentAssertions;
using Turna96.Domain.Errors;
using Turna96.Domain.ValueObjects;
using Xunit;

public class MessageBodyTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenUnderLimit()
    {
        var text = new string('a', 4096);
        var result = MessageBody.Create(text);

        result.Succeeded.Should().BeTrue();
        result.Value!.Value.Should().Be(text);
    }

    [Fact]
    public void Create_ShouldFail_WhenOverLimit()
    {
        var text = new string('a', 4097);
        var result = MessageBody.Create(text);

        result.Succeeded.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Message.BodyTooLong);
    }
}
