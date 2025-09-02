namespace Turna96.Domain.Aggregates.MessageAggregate;

using Turna96.Domain.ValueObjects;
using Turna96.SharedKernel.Domain;

public class Message : BaseEntity
{
    public Message(MessageId id, UserId senderId, RoomId roomId, string body)
    {
        Id = id;
        SenderId = senderId;
        RoomId = roomId;
        Body = body;
        AddDomainEvent(new Events.MessageSentDomainEvent(id, DateTime.UtcNow));
    }

    public MessageId Id { get; }

    public UserId SenderId { get; }

    public RoomId RoomId { get; }

    public string Body { get; }
}
