namespace Turna96.Domain.Errors;

public static partial class DomainErrors
{
    public static class Message
    {
        public static readonly Error Empty = new("Message.Empty", "Message body cannot be empty.");
        public static readonly Error TooLong = new("Message.TooLong", "Message body must be 4096 bytes or less.");
        public static readonly Error SequenceMismatch = new("Message.SequenceMismatch", "Message sequence mismatch.");
    }
}
