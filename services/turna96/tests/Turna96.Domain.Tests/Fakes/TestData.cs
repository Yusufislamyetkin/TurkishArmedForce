using Turna96.Domain.ValueObjects;

namespace Turna96.Domain.Tests.Fakes;

public static class TestData
{
    public static readonly UserId UserA = UserId.New();
    public static readonly UserId UserB = UserId.New();
    public static readonly RoomId RoomId = ValueObjects.RoomId.New();
    public static readonly MessageId MessageId = ValueObjects.MessageId.New();
    public static MessageBody ValidBody => MessageBody.Create("hello world").Value!;
}
