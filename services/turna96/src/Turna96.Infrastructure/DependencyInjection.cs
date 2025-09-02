namespace Turna96.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Turna96.Domain.Interfaces.Repositories;
using Turna96.Infrastructure.Persistence;
using Turna96.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Turna96DbContext>(options =>
        {
            // TODO: Configure provider
        });
        services.AddScoped<IMessageRepository, EfMessageRepository>();
        services.AddScoped<IRoomRepository, EfRoomRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
