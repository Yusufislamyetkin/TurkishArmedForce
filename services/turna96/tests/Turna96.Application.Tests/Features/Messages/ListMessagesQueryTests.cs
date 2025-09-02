namespace Turna96.Application.Tests.Features.Messages;

using FluentAssertions;
using Mapster;
using MapsterMapper;
using Turna96.Application.Features.Messages.Queries;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;
using Xunit;

public class ListMessagesQueryTests
{
    private readonly IMapper _mapper;

    public ListMessagesQueryTests()
    {
        var config = new TypeAdapterConfig();
        Turna96.Application.Mapping.RegisterMappings.Register(config);
        _mapper = new Mapper(config);
    }

    [Fact]
    public async Task Empty_Repository_Should_Return_Empty_List()
    {
        var repo = new InMemoryMessageRepository();
        var handler = new ListMessagesQueryHandler(repo, _mapper, new ListMessagesQueryValidator());
        var roomId = RoomId.CreateUnique();
        var response = await handler.Handle(new ListMessagesQuery(roomId, 1, 10, null), CancellationToken.None);
        response.Messages.Should().BeEmpty();
        response.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Repository_With_Messages_Should_Map_Dtos()
    {
        var repo = new InMemoryMessageRepository();
        var handler = new ListMessagesQueryHandler(repo, _mapper, new ListMessagesQueryValidator());
        var roomId = RoomId.CreateUnique();
        var sender = UserId.CreateUnique();
        var m1 = new Message(MessageId.CreateUnique(), sender, roomId, "hi", DateTime.UtcNow);
        m1.AssignSequence(1);
        var m2 = new Message(MessageId.CreateUnique(), sender, roomId, "ho", DateTime.UtcNow);
        m2.AssignSequence(2);
        await repo.AddAsync(m1, default);
        await repo.AddAsync(m2, default);

        var response = await handler.Handle(new ListMessagesQuery(roomId, 1, 10, null), CancellationToken.None);
        response.Messages.Should().HaveCount(2);
        response.Messages[0].Body.Should().Be("hi");
        response.TotalCount.Should().Be(2);
    }

    [Fact]
    public void Page_Must_Be_Greater_Than_Zero()
    {
        var validator = new ListMessagesQueryValidator();
        var result = validator.Validate(new ListMessagesQuery(RoomId.CreateUnique(), 0, 10, null));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void PageSize_Must_Not_Exceed_100()
    {
        var validator = new ListMessagesQueryValidator();
        var result = validator.Validate(new ListMessagesQuery(RoomId.CreateUnique(), 1, 101, null));
        result.IsValid.Should().BeFalse();
    }

    private sealed class InMemoryMessageRepository : IMessageRepository
    {
        private readonly List<Message> _messages = new();

        public Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            _messages.Add(message);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Message>> ListAsync(RoomId roomId, CancellationToken cancellationToken)
        {
            IReadOnlyList<Message> list = _messages.Where(m => m.RoomId == roomId).ToList();
            return Task.FromResult(list);
        }
    }
}
