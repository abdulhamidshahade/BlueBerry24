using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;

namespace BlueBerry24.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;
        public UnitOfWork(ApplicationDbContext context
                          )
        {
            _context = context;
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

        public IExecutionStrategy BeginTransactionAsyncStrategy()
        {
            return _context.Database.CreateExecutionStrategy();
        }
    }
}
