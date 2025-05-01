﻿using BlueBerry24.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlueBerry24.Domain.Repositories
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;
        public UnitOfWork(ApplicationDbContext context, IDbContextTransaction currentTransaction)
        {
            _context = context;
            _currentTransaction = currentTransaction;
        }

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
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
