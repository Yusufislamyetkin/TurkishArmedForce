namespace Turna96.Application.Abstractions;

using Turna96.Domain.ValueObjects;

public interface ICurrentUserService
{
    UserId GetCurrentUserId();
}
