using System.Threading;
using System.Threading.Tasks;

namespace Turna96.Domain.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
