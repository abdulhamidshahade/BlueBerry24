using Berryfy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Berryfy.Domain.Repositories
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
            if (_currentTransaction != null)
            {
                return;
            }
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        

        public async Task<bool> CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                return false;
            }

            try
            {
                await _currentTransaction.CommitAsync();
                return true;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
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
