using Turna96.Domain.ValueObjects;
using Turna96.Domain.Errors;
using Xunit;

namespace Turna96.Domain.Tests.ValueObjects;

public class MessageBodyTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenWithinLimit()
    {
        var result = MessageBody.Create(new string('a', 10));
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value!.Value.Length);
    }

    [Fact]
    public void Create_Should_Fail_WhenExceedsLimit()
    {
        var result = MessageBody.Create(new string('a', 4097));
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Message.TooLong, result.Error);
    }
}
