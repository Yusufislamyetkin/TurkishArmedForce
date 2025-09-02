namespace Turna96.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Aggregates.RoomAggregate;

public class Turna96DbContext : DbContext
{
    public Turna96DbContext(DbContextOptions<Turna96DbContext> options)
        : base(options)
    {
    }

    public DbSet<Message> Messages => Set<Message>();

    public DbSet<Room> Rooms => Set<Room>();

    // TODO: Configure model
}
