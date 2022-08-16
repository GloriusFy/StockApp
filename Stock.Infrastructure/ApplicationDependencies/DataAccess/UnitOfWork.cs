using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Stock.Application.Common.Dependency.DataAccess;
using Stock.Application.Common.Dependency.DataAccess.Repositories;
using Stock.Infrastructure.Persistence.Context;

namespace Stock.Infrastructure.ApplicationDependencies.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(ApplicationDbContext dbContext, IPartnerRepository partners, 
        IProductRepository products, ITransactionRepository transactions)
    {
        _dbContext = dbContext;
        Partners = partners;
        Products = products;
        Transactions = transactions;
    }

    public void Dispose() 
        => _dbContext.Dispose();

    public IPartnerRepository Partners { get; }
    public IProductRepository Products { get; }
    public ITransactionRepository Transactions { get; }
    public bool HasActiveTransaction 
        => _currentTransaction != null!;
    
    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();

            _currentTransaction?.Commit();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _currentTransaction?.RollbackAsync();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public Task SaveChanges() 
        => _dbContext.SaveChangesAsync();
    
}