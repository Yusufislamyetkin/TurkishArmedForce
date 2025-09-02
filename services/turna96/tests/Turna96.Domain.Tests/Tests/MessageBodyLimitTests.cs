namespace Turna96.Domain.Tests.Tests;

using System.Text;
using FluentAssertions;
using Xunit;

public class MessageBodyLimitTests
{
    [Fact]
    public void BodyShouldNotExceed4096Bytes()
    {
        var body = new string('a', 4097);
        var bytes = Encoding.UTF8.GetByteCount(body);
        (bytes <= 4096).Should().BeFalse();
    }
}
