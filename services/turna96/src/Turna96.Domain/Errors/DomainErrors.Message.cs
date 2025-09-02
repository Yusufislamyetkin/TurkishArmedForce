namespace Turna96.Domain.Errors;

public static partial class DomainErrors
{
    public static class Message
    {
        public static readonly Error BodyTooLong = new("Message.BodyTooLong", "Message body cannot exceed 4096 bytes.");

        public static Error SequenceMismatch(long current, long incoming) =>
            new("Message.SequenceMismatch", $"Sequence {incoming} does not match current {current}.");
    }
}
