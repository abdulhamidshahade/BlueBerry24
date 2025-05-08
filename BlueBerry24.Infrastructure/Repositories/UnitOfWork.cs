using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;

namespace BlueBerry24.Domain.Repositories
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;
        private ITransaction _cacheTransaction;
        private readonly StackExchange.Redis.IDatabase _db;
        public UnitOfWork(ApplicationDbContext context, 
                          IDbContextTransaction currentTransaction, 
                          ITransaction cacheTransaction,
                          IConnectionMultiplexer redis)
        {
            _context = context;
            _currentTransaction = currentTransaction;
            _cacheTransaction = cacheTransaction;
            _db = redis.GetDatabase();
        }


        public void BeginCacheTransaction()
        {
            _cacheTransaction = _db.CreateTransaction();
        }

        public async Task<bool> CacheCommitTransactionAsync()
        {
            if(_cacheTransaction == null)
            {
                return false;
            }

            return await _cacheTransaction.ExecuteAsync();
        }


        public async Task ExecuteInTransactionCacheAsync(Func<ITransaction, Task> action)
        {
            if(_cacheTransaction == null)
            {
                BeginCacheTransaction();
            }

            await action(_cacheTransaction!);
        }



        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        

        public async Task<bool> CommitTransactionAsync()
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();

            return true;
        }

        

        public async Task RollbackTransactionAsync()
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
        }

        public async Task<bool> SaveDbChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
