namespace Turna96.Domain.Interfaces.Repositories;

using Turna96.Domain.Aggregates.RoomAggregate;
using Turna96.Domain.ValueObjects;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(RoomId id, CancellationToken cancellationToken);

    Task AddAsync(Room room, CancellationToken cancellationToken);
}
