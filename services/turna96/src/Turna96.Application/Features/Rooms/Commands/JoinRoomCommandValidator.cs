namespace Turna96.Application.Features.Rooms.Commands;

using FluentValidation;

public sealed class JoinRoomCommandValidator : AbstractValidator<JoinRoomCommand>
{
    public JoinRoomCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull();
    }
}
