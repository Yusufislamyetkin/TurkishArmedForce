using System.Text;

namespace Turna96.Domain.ValueObjects;

public sealed class MessageBody : ValueObject
{
    public string Value { get; }

    private MessageBody(string value) => Value = value;

    public static Result<MessageBody> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<MessageBody>.Failure(DomainErrors.Message.Empty);

        if (Encoding.UTF8.GetByteCount(value) > 4096)
            return Result<MessageBody>.Failure(DomainErrors.Message.TooLong);

        return Result<MessageBody>.Success(new MessageBody(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
