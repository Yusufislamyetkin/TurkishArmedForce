namespace Turna96.Domain.Aggregates.RoomAggregate;

public sealed class RoomMember
{
    public UserId UserId { get; }
    public DateTime JoinedAt { get; }

    public RoomMember(UserId userId, DateTime joinedAt)
    {
        UserId = userId;
        JoinedAt = joinedAt;
    }
}
