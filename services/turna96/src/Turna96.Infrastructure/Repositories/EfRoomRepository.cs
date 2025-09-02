namespace Turna96.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Turna96.Domain.Aggregates.RoomAggregate;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;
using Turna96.Infrastructure.Persistence;

public class EfRoomRepository : IRoomRepository
{
    private readonly Turna96DbContext _context;

    public EfRoomRepository(Turna96DbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetByIdAsync(RoomId id, CancellationToken cancellationToken)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task AddAsync(Room room, CancellationToken cancellationToken)
    {
        _context.Rooms.Add(room);
        return Task.CompletedTask;
    }
}
