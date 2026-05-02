using System;

namespace DDD.SharedKernel.DomainModelLayer
{
    public interface IDomainEvent
    {
        DateTime Created { get; }
    }
}
