using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveDbChangesAsync();

        void BeginCacheTransaction();
        Task<bool> CacheCommitTransactionAsync();
        Task ExecuteInTransactionCacheAsync(Func<ITransaction, Task> action);
    }
}
