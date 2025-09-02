namespace Turna96.Domain.Aggregates.RoomAggregate.Members;

using Turna96.Domain.ValueObjects;

public class RoomMember
{
    public RoomMember(UserId userId)
    {
        UserId = userId;
    }

    public UserId UserId { get; }
}
