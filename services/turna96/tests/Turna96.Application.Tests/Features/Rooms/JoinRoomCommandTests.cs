namespace Turna96.Application.Tests.Features.Rooms;

using FluentAssertions;
using Turna96.Application.Features.Rooms.Commands;
using Turna96.Domain.Aggregates.RoomAggregate;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;
using Xunit;

public class JoinRoomCommandTests
{
    [Fact]
    public async Task First_Join_Should_Add_Member()
    {
        var room = new Room(RoomId.CreateUnique());
        var repo = new InMemoryRoomRepository(room);
        var handler = new JoinRoomCommandHandler(repo, new FakeUnitOfWork(), new JoinRoomCommandValidator());
        var userId = UserId.CreateUnique();

        var response = await handler.Handle(new JoinRoomCommand(room.Id, userId), CancellationToken.None);

        response.Success.Should().BeTrue();
        room.Members.Should().ContainSingle(m => m.UserId == userId);
    }

    [Fact]
    public async Task Repeated_Join_Should_Be_Idempotent()
    {
        var room = new Room(RoomId.CreateUnique());
        var repo = new InMemoryRoomRepository(room);
        var handler = new JoinRoomCommandHandler(repo, new FakeUnitOfWork(), new JoinRoomCommandValidator());
        var userId = UserId.CreateUnique();

        await handler.Handle(new JoinRoomCommand(room.Id, userId), CancellationToken.None);
        await handler.Handle(new JoinRoomCommand(room.Id, userId), CancellationToken.None);

        room.Members.Should().ContainSingle(m => m.UserId == userId);
    }

    private sealed class InMemoryRoomRepository : IRoomRepository
    {
        private readonly Room _room;
        public InMemoryRoomRepository(Room room) => _room = room;

        public Task<Room?> GetByIdAsync(RoomId id, CancellationToken cancellationToken)
            => Task.FromResult<Room?>(id == _room.Id ? _room : null);

        public Task AddAsync(Room room, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => Task.FromResult(1);
    }
}
