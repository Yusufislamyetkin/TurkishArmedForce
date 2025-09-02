namespace Turna96.Application.Features.JoinRoom;

using FluentValidation;

public sealed record JoinRoomCommand(string RoomId);

public class JoinRoomCommandValidator : AbstractValidator<JoinRoomCommand>
{
    public JoinRoomCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty();
    }
}
