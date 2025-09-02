namespace Turna96.Application.Features.Rooms.Commands;

using FluentValidation;
using Turna96.Application.Contracts.Rooms;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;

public sealed record JoinRoomCommand(RoomId RoomId, UserId UserId);

public sealed class JoinRoomCommandHandler
{
    private readonly IRoomRepository _rooms;
    private readonly IUnitOfWork _uow;
    private readonly IValidator<JoinRoomCommand> _validator;

    public JoinRoomCommandHandler(
        IRoomRepository rooms,
        IUnitOfWork uow,
        IValidator<JoinRoomCommand> validator)
    {
        _rooms = rooms;
        _uow = uow;
        _validator = validator;
    }

    public async Task<JoinRoomResponse> Handle(JoinRoomCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        var room = await _rooms.GetByIdAsync(command.RoomId, ct);
        if (room is null)
        {
            return new JoinRoomResponse(false, "Room not found");
        }

        var before = room.Members.Count;
        room.Join(command.UserId);
        var after = room.Members.Count;

        await _uow.SaveChangesAsync(ct);

        var already = before == after;
        var message = already ? "Already joined" : "Joined";
        return new JoinRoomResponse(true, message);
    }
}
