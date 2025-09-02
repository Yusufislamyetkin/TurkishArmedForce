using System.Threading;
using System.Threading.Tasks;

namespace Turna96.Domain.Interfaces.Repositories;

public interface IMessageRepository
{
    Task AddAsync(Message message, CancellationToken cancellationToken = default);
    Task<Message?> GetByIdAsync(MessageId id, CancellationToken cancellationToken = default);
}
