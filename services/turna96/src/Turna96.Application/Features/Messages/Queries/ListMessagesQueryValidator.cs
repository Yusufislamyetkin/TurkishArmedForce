namespace Turna96.Application.Features.Messages.Queries;

using FluentValidation;

public sealed class ListMessagesQueryValidator : AbstractValidator<ListMessagesQuery>
{
    public ListMessagesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
    }
}
