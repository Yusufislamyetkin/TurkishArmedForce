using Turna96.Domain.Aggregates.RoomAggregate.Events;

namespace Turna96.Domain.Aggregates.RoomAggregate;

public sealed class Room : Entity
{
    private readonly Dictionary<UserId, RoomMember> _members = new();

    private Room(RoomId id, RoomType type)
    {
        Id = id;
        Type = type;
    }

    public RoomId Id { get; }
    public RoomType Type { get; }
    public IReadOnlyCollection<RoomMember> Members => _members.Values.ToList().AsReadOnly();

    public static Room Create(RoomId id, RoomType type) => new(id, type);

    public Result Join(UserId userId, IClock clock)
    {
        if (_members.ContainsKey(userId))
            return Result.Success();

        var member = new RoomMember(userId, clock.UtcNow);
        _members.Add(userId, member);
        RaiseDomainEvent(new RoomMemberJoined(Id, userId, member.JoinedAt));
        return Result.Success();
    }

    public Result Leave(UserId userId, IClock clock)
    {
        if (!_members.ContainsKey(userId))
            return Result.Success();

        _members.Remove(userId);
        RaiseDomainEvent(new RoomMemberLeft(Id, userId, clock.UtcNow));
        return Result.Success();
    }
}
