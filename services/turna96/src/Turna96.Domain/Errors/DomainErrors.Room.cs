namespace Turna96.Domain.Errors;

public static partial class DomainErrors
{
    public static class Room
    {
        public static readonly Error MemberNotFound = new("Room.MemberNotFound", "Member not found in room.");
    }
}
