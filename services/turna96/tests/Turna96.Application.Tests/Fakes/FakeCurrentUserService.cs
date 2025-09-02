namespace Turna96.Application.Tests.Fakes;

using Turna96.Application.Abstractions;

public sealed class FakeCurrentUserService : ICurrentUserService
{
    public FakeCurrentUserService(string? userId)
    {
        UserId = userId;
    }

    public string? UserId { get; }
}
