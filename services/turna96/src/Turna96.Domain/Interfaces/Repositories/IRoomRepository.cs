using System.Threading;
using System.Threading.Tasks;

namespace Turna96.Domain.Interfaces.Repositories;

public interface IRoomRepository
{
    Task AddAsync(Room room, CancellationToken cancellationToken = default);
    Task<Room?> GetByIdAsync(RoomId id, CancellationToken cancellationToken = default);
}
