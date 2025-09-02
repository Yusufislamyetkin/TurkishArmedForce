namespace Turna96.Domain.ValueObjects;

using System.Text;
using Turna96.Domain.Errors;

public sealed class MessageBody : ValueObject
{
    private MessageBody(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<MessageBody> Create(string value)
    {
        var bytes = Encoding.UTF8.GetByteCount(value);
        if (bytes > 4096)
        {
            return Result<MessageBody>.Failure(DomainErrors.Message.BodyTooLong);
        }

        return Result<MessageBody>.Success(new MessageBody(value));
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
