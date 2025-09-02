namespace Turna96.SharedKernel.Domain;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
