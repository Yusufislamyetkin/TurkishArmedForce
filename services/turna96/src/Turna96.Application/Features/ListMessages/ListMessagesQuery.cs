namespace Turna96.Application.Features.ListMessages;

using FluentValidation;

public sealed record ListMessagesQuery(string RoomId);

public class ListMessagesQueryValidator : AbstractValidator<ListMessagesQuery>
{
    public ListMessagesQueryValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty();
    }
}
