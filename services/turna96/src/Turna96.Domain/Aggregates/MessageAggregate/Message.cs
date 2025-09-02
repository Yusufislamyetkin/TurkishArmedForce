using Turna96.Domain.Aggregates.MessageAggregate.Events;

namespace Turna96.Domain.Aggregates.MessageAggregate;

public sealed class Message : Entity
{
    public MessageId Id { get; }
    public RoomId RoomId { get; }
    public UserId SenderId { get; }
    public MessageBody Body { get; }
    public string? Metadata { get; }
    public long? Sequence { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Message(MessageId id, RoomId roomId, UserId senderId, MessageBody body, string? metadata, DateTime createdAt)
    {
        Id = id;
        RoomId = roomId;
        SenderId = senderId;
        Body = body;
        Metadata = metadata;
        CreatedAt = createdAt;
    }

    public static Message Create(MessageId id, RoomId roomId, UserId senderId, MessageBody body, string? metadata, IClock clock)
        => new(id, roomId, senderId, body, metadata, clock.UtcNow);

    public Result SetSequence(long sequence)
    {
        if (Sequence is null)
        {
            Sequence = sequence;
            RaiseDomainEvent(new MessageSent(Id, RoomId, SenderId, sequence, CreatedAt));
            return Result.Success();
        }

        if (Sequence == sequence)
            return Result.Success();

        return Result.Failure(DomainErrors.Message.SequenceMismatch);
    }

    public Result MarkDelivered(IClock clock)
    {
        if (DeletedAt is not null || DeliveredAt is not null)
            return Result.Success();

        DeliveredAt = clock.UtcNow;
        RaiseDomainEvent(new MessageDelivered(Id, RoomId, Sequence!.Value, DeliveredAt.Value));
        return Result.Success();
    }

    public Result MarkRead(IClock clock)
    {
        if (DeletedAt is not null || ReadAt is not null)
            return Result.Success();

        ReadAt = clock.UtcNow;
        RaiseDomainEvent(new MessageRead(Id, RoomId, Sequence!.Value, ReadAt.Value));
        return Result.Success();
    }

    public Result Delete(IClock clock)
    {
        if (DeletedAt is not null)
            return Result.Success();

        DeletedAt = clock.UtcNow;
        return Result.Success();
    }
}
