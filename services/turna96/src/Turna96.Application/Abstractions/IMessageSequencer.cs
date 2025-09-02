namespace Turna96.Application.Abstractions;

public interface IMessageSequencer
{
    Task<long> NextAsync(CancellationToken cancellationToken);
}
