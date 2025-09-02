namespace Turna96.Application.Mapping;

using Mapster;
using Turna96.Application.Contracts.Messages;
using Turna96.Domain.Aggregates.MessageAggregate;

public static class RegisterMappings
{
    public static void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Message, MessageDto>()
            .Map(dest => dest.MessageId, src => src.Id)
            .Map(dest => dest.RoomId, src => src.RoomId)
            .Map(dest => dest.SenderId, src => src.SenderId)
            .Map(dest => dest.Body, src => src.Body)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.DeliveredAt, src => src.DeliveredAt)
            .Map(dest => dest.ReadAt, src => src.ReadAt)
            .Map(dest => dest.DeletedAt, src => src.DeletedAt);
    }
}
