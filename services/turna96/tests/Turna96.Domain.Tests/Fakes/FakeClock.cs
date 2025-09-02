using Turna96.Domain.Services;

namespace Turna96.Domain.Tests.Fakes;

public sealed class FakeClock : IClock
{
    public DateTime UtcNow { get; set; }

    public FakeClock(DateTime? utcNow = null)
    {
        UtcNow = utcNow ?? DateTime.UtcNow;
    }

    public void Advance(TimeSpan span) => UtcNow = UtcNow.Add(span);
}
