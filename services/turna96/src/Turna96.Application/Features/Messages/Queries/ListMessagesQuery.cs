namespace Turna96.Application.Features.Messages.Queries;

using FluentValidation;
using MapsterMapper;
using Turna96.Application.Contracts.Messages;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;

public sealed record ListMessagesQuery(RoomId RoomId, int Page, int PageSize, long? AfterSequence);

public sealed class ListMessagesQueryHandler
{
    private readonly IMessageRepository _messages;
    private readonly IMapper _mapper;
    private readonly IValidator<ListMessagesQuery> _validator;

    public ListMessagesQueryHandler(
        IMessageRepository messages,
        IMapper mapper,
        IValidator<ListMessagesQuery> validator)
    {
        _messages = messages;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ListMessagesResponse> Handle(ListMessagesQuery query, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(query, ct);

        var all = await _messages.ListAsync(query.RoomId, ct);
        var filtered = query.AfterSequence.HasValue
            ? all.Where(m => m.Sequence > query.AfterSequence.Value).ToList()
            : all.ToList();

        var paged = filtered
            .OrderBy(m => m.Sequence)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<MessageDto>>(paged);
        return new ListMessagesResponse(dtos, filtered.Count);
    }
}
