namespace DDD.SharedKernel.DomainModelLayer
{
    public interface IDomainEventPublisher
    {
        void Publish<T>(T domainEvent) 
            where T : IDomainEvent;
    }
}
