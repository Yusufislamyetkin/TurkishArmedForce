namespace Turna96.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;
using Turna96.Infrastructure.Persistence;

public class EfMessageRepository : IMessageRepository
{
    private readonly Turna96DbContext _context;

    public EfMessageRepository(Turna96DbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Message message, CancellationToken cancellationToken)
    {
        _context.Messages.Add(message);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<Message>> ListAsync(RoomId roomId, CancellationToken cancellationToken)
    {
        return await _context.Messages.Where(m => m.RoomId == roomId).ToListAsync(cancellationToken);
    }
}
