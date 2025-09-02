namespace Turna96.Domain.Aggregates.RoomAggregate;

using Turna96.Domain.Aggregates.RoomAggregate.Events;
using Turna96.Domain.Aggregates.RoomAggregate.Members;
using Turna96.Domain.ValueObjects;
using Turna96.SharedKernel.Domain;

public class Room : BaseEntity
{
    private readonly List<RoomMember> _members = new();

    public Room(RoomId id)
    {
        Id = id;
    }

    public RoomId Id { get; }

    public IReadOnlyCollection<RoomMember> Members => _members.AsReadOnly();

    public void Join(UserId userId)
    {
        if (_members.Any(m => m.UserId == userId))
        {
            return;
        }

        var member = new RoomMember(userId);
        _members.Add(member);
        AddDomainEvent(new RoomJoinedDomainEvent(Id, userId, DateTime.UtcNow));
    }
}
