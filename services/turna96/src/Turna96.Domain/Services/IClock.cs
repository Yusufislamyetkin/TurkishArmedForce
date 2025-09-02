namespace Turna96.Domain.Services;

public interface IClock
{
    DateTime UtcNow { get; }
}
