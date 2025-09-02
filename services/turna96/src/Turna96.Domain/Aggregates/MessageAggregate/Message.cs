namespace Turna96.Domain.Aggregates.MessageAggregate;

using Turna96.Domain.ValueObjects;
using Turna96.SharedKernel.Domain;

public class Message : BaseEntity
{
    public Message(MessageId id, UserId senderId, RoomId roomId, string body, DateTime createdAt)
    {
        Id = id;
        SenderId = senderId;
        RoomId = roomId;
        Body = body;
        CreatedAt = createdAt;
        AddDomainEvent(new Events.MessageSentDomainEvent(id, createdAt));
    }

    public MessageId Id { get; }

    public UserId SenderId { get; }

    public RoomId RoomId { get; }

    public string Body { get; }

    public long Sequence { get; private set; }

    public DateTime CreatedAt { get; }

    public DateTime? DeliveredAt { get; private set; }

    public DateTime? ReadAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public void AssignSequence(long sequence) => Sequence = sequence;
}
