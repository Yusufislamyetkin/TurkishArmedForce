using System.Threading;
using System.Threading.Tasks;

namespace Turna96.Domain.Services;

public interface IMessageSequencer
{
    Task<long> NextAsync(RoomId roomId, CancellationToken cancellationToken = default);
}
