namespace Turna96.Domain.Aggregates.RoomAggregate;

using Turna96.Domain.Aggregates.RoomAggregate.Events;
using Turna96.Domain.Enums;
using Turna96.Domain.Services;
using Turna96.Domain.ValueObjects;

public sealed class Room : BaseEntity
{
    private readonly List<RoomMember> _members = new();

    public Room(RoomId id, RoomType type)
    {
        Id = id;
        Type = type;
    }

    public RoomId Id { get; }

    public RoomType Type { get; }

    public IReadOnlyCollection<RoomMember> Members => _members.AsReadOnly();

    public Result Join(UserId userId, IClock clock)
    {
        if (_members.Any(m => m.UserId == userId))
        {
            return Result.Success();
        }

        var member = new RoomMember(userId);
        _members.Add(member);
        AddDomainEvent(new RoomMemberJoinedDomainEvent(Id, userId, clock.UtcNow));
        return Result.Success();
    }

    public Result Leave(UserId userId, IClock clock)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member is null)
        {
            return Result.Success();
        }

        _members.Remove(member);
        AddDomainEvent(new RoomMemberLeftDomainEvent(Id, userId, clock.UtcNow));
        return Result.Success();
    }
}
