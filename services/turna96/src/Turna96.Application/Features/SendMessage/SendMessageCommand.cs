namespace Turna96.Application.Features.SendMessage;

using FluentValidation;

public sealed record SendMessageCommand(string RoomId, string Body);

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Body).NotEmpty();
    }
}
