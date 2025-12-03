using Microsoft.EntityFrameworkCore.Storage;

namespace BlueBerry24.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveDbChangesAsync();
        IExecutionStrategy BeginTransactionAsyncStrategy();
    }
}
