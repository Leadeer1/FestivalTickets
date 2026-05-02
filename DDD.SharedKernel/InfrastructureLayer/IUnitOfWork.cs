using System;

namespace DDD.SharedKernel.InfrastructureLayer
{
    public interface IUnitOfWork: IDisposable
    {
        void Commit();
        void RejectChanges();
    }
}
