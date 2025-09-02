namespace Turna96.Domain.Aggregates.RoomAggregate;

using Turna96.Domain.ValueObjects;

public sealed class RoomMember
{
    public RoomMember(UserId userId)
    {
        UserId = userId;
    }

    public UserId UserId { get; }
}
