using Microsoft.EntityFrameworkCore.Storage;

namespace Berryfy.Domain.Repositories
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
