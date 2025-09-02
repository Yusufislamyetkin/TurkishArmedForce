namespace Turna96.Application.Tests.Features.Messages;

using FluentAssertions;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Turna96.Application.Abstractions;
using Turna96.Application.Features.Messages.Commands;
using Turna96.Application.Tests.Fakes;
using Turna96.Domain.Aggregates.MessageAggregate;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Domain.ValueObjects;
using Xunit;

public class SendMessageCommandTests
{
    private readonly IMapper _mapper;

    public SendMessageCommandTests()
    {
        var config = new TypeAdapterConfig();
        Turna96.Application.Mapping.RegisterMappings.Register(config);
        _mapper = new Mapper(config);
    }

    [Fact]
    public async Task Valid_Request_Should_Create_Message()
    {
        var repo = new InMemoryMessageRepository();
        var uow = new FakeUnitOfWork();
        var user = new FakeCurrentUserService(Guid.NewGuid().ToString());
        var sequencer = new FakeMessageSequencer();
        var validator = new SendMessageCommandValidator();
        var handler = new SendMessageCommandHandler(repo, uow, user, sequencer, _mapper, validator);

        var roomId = RoomId.CreateUnique();
        var command = new SendMessageCommand(roomId, "hello", null);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Sequence.Should().Be(1);
        repo.Messages.Should().HaveCount(1);
        repo.Messages[0].Sequence.Should().Be(1);
    }

    [Fact]
    public async Task Empty_Body_Should_Fail()
    {
        var handler = BuildHandler();
        var roomId = RoomId.CreateUnique();
        var command = new SendMessageCommand(roomId, string.Empty, null);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Body_Larger_Than_4096_Bytes_Should_Fail()
    {
        var handler = BuildHandler();
        var roomId = RoomId.CreateUnique();
        var big = new string('a', 4097);
        var command = new SendMessageCommand(roomId, big, null);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    private SendMessageCommandHandler BuildHandler()
    {
        var repo = new InMemoryMessageRepository();
        var uow = new FakeUnitOfWork();
        var user = new FakeCurrentUserService(Guid.NewGuid().ToString());
        var sequencer = new FakeMessageSequencer();
        var validator = new SendMessageCommandValidator();
        return new SendMessageCommandHandler(repo, uow, user, sequencer, _mapper, validator);
    }

    private sealed class InMemoryMessageRepository : IMessageRepository
    {
        public List<Message> Messages { get; } = new();

        public Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            Messages.Add(message);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Message>> ListAsync(RoomId roomId, CancellationToken cancellationToken)
        {
            IReadOnlyList<Message> list = Messages.Where(m => m.RoomId == roomId).ToList();
            return Task.FromResult(list);
        }
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => Task.FromResult(1);
    }
}
