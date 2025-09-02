namespace Turna96.Application.Features.Messages.Commands;

using System.Text;
using FluentValidation;

public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty()
            .Must(body => Encoding.UTF8.GetByteCount(body) <= 4096)
            .WithMessage("Body must be at most 4096 bytes.");
    }
}
