namespace Turna96.SharedKernel.Time;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
