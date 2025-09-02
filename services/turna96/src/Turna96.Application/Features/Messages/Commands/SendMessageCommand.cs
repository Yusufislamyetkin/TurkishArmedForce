namespace Turna96.Application.Features.Messages.Commands;

using FluentValidation;
using MapsterMapper;
using Turna96.Application.Abstractions;
using Turna96.Application.Contracts.Messages;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;

public sealed record SendMessageCommand(RoomId RoomId, string Body, string? MetadataJson);

public sealed class SendMessageCommandHandler
{
    private readonly IMessageRepository _messages;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly IMessageSequencer _sequencer;
    private readonly IMapper _mapper;
    private readonly IValidator<SendMessageCommand> _validator;

    public SendMessageCommandHandler(
        IMessageRepository messages,
        IUnitOfWork uow,
        ICurrentUserService currentUser,
        IMessageSequencer sequencer,
        IMapper mapper,
        IValidator<SendMessageCommand> validator)
    {
        _messages = messages;
        _uow = uow;
        _currentUser = currentUser;
        _sequencer = sequencer;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<SendMessageResponse> Handle(SendMessageCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        var senderId = new UserId(Guid.Parse(_currentUser.UserId ?? throw new InvalidOperationException("User not found")));
        var message = new Message(MessageId.CreateUnique(), senderId, command.RoomId, command.Body, DateTime.UtcNow);

        await _messages.AddAsync(message, ct);
        var sequence = await _sequencer.NextAsync(command.RoomId, ct);
        message.AssignSequence(sequence);

        await _uow.SaveChangesAsync(ct);

        var dto = _mapper.Map<MessageDto>(message);
        return new SendMessageResponse(message.Id, sequence, message.CreatedAt);
    }
}
