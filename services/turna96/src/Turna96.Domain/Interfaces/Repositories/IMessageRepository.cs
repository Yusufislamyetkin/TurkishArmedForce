namespace Turna96.Domain.Interfaces.Repositories;

using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.ValueObjects;

public interface IMessageRepository
{
    Task AddAsync(Message message, CancellationToken cancellationToken);

    Task<IReadOnlyList<Message>> ListAsync(RoomId roomId, CancellationToken cancellationToken);
}
