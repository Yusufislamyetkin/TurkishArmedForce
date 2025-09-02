namespace Turna96.Domain.Aggregates.MessageAggregate;

using Turna96.Domain.Aggregates.MessageAggregate.Events;
using Turna96.Domain.Errors;
using Turna96.Domain.Services;
using Turna96.Domain.ValueObjects;

public sealed class Message : BaseEntity
{
    private Message(
        MessageId id,
        UserId senderId,
        RoomId roomId,
        MessageBody body,
        DateTime createdAt,
        string? metadata)
    {
        Id = id;
        SenderId = senderId;
        RoomId = roomId;
        Body = body;
        CreatedAt = createdAt;
        Metadata = metadata;
        AddDomainEvent(new MessageSentDomainEvent(id, roomId, senderId, createdAt));
    }

    public MessageId Id { get; }

    public UserId SenderId { get; }

    public RoomId RoomId { get; }

    public MessageBody Body { get; }

    public long? Sequence { get; private set; }

    public string? Metadata { get; }

    public DateTime CreatedAt { get; }

    public DateTime? DeliveredAt { get; private set; }

    public DateTime? ReadAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public static Result<Message> Create(
        MessageId id,
        UserId senderId,
        RoomId roomId,
        MessageBody body,
        IClock clock,
        string? metadata = null)
    {
        var message = new Message(id, senderId, roomId, body, clock.UtcNow, metadata);
        return Result<Message>.Success(message);
    }

    public Result AssignSequence(long sequence)
    {
        if (Sequence is null)
        {
            Sequence = sequence;
            return Result.Success();
        }

        return Sequence == sequence
            ? Result.Success()
            : Result.Failure(DomainErrors.Message.SequenceMismatch(Sequence.Value, sequence));
    }

    public Result MarkDelivered(IClock clock)
    {
        if (DeletedAt is not null)
        {
            return Result.Success();
        }

        if (DeliveredAt is not null)
        {
            return Result.Success();
        }

        DeliveredAt = clock.UtcNow;
        AddDomainEvent(new MessageDeliveredDomainEvent(Id, DeliveredAt.Value));
        return Result.Success();
    }

    public Result MarkRead(IClock clock)
    {
        if (DeletedAt is not null)
        {
            return Result.Success();
        }

        if (ReadAt is not null)
        {
            return Result.Success();
        }

        ReadAt = clock.UtcNow;
        AddDomainEvent(new MessageReadDomainEvent(Id, ReadAt.Value));
        return Result.Success();
    }

    public Result Delete(IClock clock)
    {
        if (DeletedAt is not null)
        {
            return Result.Success();
        }

        DeletedAt = clock.UtcNow;
        return Result.Success();
    }
}
