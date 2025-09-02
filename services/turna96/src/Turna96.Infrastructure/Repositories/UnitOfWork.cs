namespace Turna96.Infrastructure.Repositories;

using Turna96.Domain.Interfaces.Repositories;
using Turna96.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly Turna96DbContext _context;

    public UnitOfWork(Turna96DbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
