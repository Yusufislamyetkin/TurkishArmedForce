namespace Turna96.Domain.Errors;

public static partial class DomainErrors
{
    public static class Room
    {
        public static readonly Error MemberAlreadyExists = new("Room.MemberAlreadyExists", "User already joined the room.");
        public static readonly Error MemberNotFound = new("Room.MemberNotFound", "User is not a member of the room.");
    }
}
