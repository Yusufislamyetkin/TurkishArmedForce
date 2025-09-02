namespace Turna96.Application.Tests.Tests;

using FluentAssertions;
using Turna96.Application.Features.SendMessage;
using Xunit;

public class SendMessageValidatorTests
{
    private readonly SendMessageCommandValidator _validator = new();

    [Fact]
    public void Should_Fail_When_Body_Is_Empty()
    {
        var result = _validator.Validate(new SendMessageCommand("room", string.Empty));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Body_Is_Not_Empty()
    {
        var result = _validator.Validate(new SendMessageCommand("room", "hi"));
        result.IsValid.Should().BeTrue();
    }
}
