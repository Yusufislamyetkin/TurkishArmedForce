namespace Turna96.Domain.Tests.Fakes;

using Turna96.Domain.ValueObjects;

public static class TestData
{
    public static UserId UserId() => new(Guid.Parse("00000000-0000-0000-0000-000000000001"));

    public static RoomId RoomId() => new(Guid.Parse("00000000-0000-0000-0000-000000000002"));

    public static MessageId MessageId() => new(Guid.Parse("00000000-0000-0000-0000-000000000003"));

    public static MessageBody MessageBody(string text = "hello")
        => Turna96.Domain.ValueObjects.MessageBody.Create(text).Value!;
}
